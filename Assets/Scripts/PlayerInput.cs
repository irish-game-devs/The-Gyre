using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharMotor))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool stefano = false;

    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float rotSpeed = 8.0f;

    private Transform tf;
    private Transform playCamTf;

    private const string horInputTwo = "HorizontalTwo";
    private const string verInputTwo = "VerticalTwo";

    private Vector3 facing = Vector3.zero;

    private float   usingControllerTimer = 0.0f;
    private float   usingControllerMax = 1.0f;

    private CharMotor motor;
    private PlayerCam playCam;

    private void Awake()
    {
        tf  = GetComponent<Transform>();
        motor = GetComponent<CharMotor>();
    }

    private void Start()
    {
        playCamTf = GameObject.Find("Main Camera").GetComponent<Transform>();
        playCam = playCamTf.GetComponent<PlayerCam>();
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 desiredMoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        Vector3 desiredLookDir = new Vector3(Input.GetAxisRaw(horInputTwo), 0.0f, Input.GetAxisRaw(verInputTwo));

        Vector3 mouseMovement = new Vector3(Input.GetAxisRaw("Mouse X"), 0.0f, Input.GetAxisRaw("Mouse Y"));

        HandleFacing(desiredMoveDir, desiredLookDir, mouseMovement);

        Vector3 desiredMove = Vector3.zero;

        if(Mathf.Abs(desiredMoveDir.x) >= 0.1f || Mathf.Abs(desiredMoveDir.z) >= 0.1f)
        {
            desiredMove = MakeDirectionCamRelative(desiredMoveDir).normalized * moveSpeed * Time.deltaTime;
            // desiredMove = desiredMoveDir.normalized * moveSpeed * Time.deltaTime;
        }
        
        motor.ReceiveInput(desiredMove, facing);
    }

    void HandleFacing(Vector3 desiredMoveDir, Vector3 desiredLookDir, Vector3 mouseMovement)
    {
        if (Mathf.Abs(mouseMovement.x) >= 0.001f || Mathf.Abs(mouseMovement.z) >= 0.001f)
        {
            usingControllerTimer = usingControllerMax;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(playCamTf.position, tf.position);

            Vector3 v3 = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 desiredLookDirMouse = v3 - tf.position;

            facing = MakeDirectionCamRelative(desiredLookDirMouse);
        }
        else if (Mathf.Abs(desiredLookDir.x) >= 0.001f || Mathf.Abs(desiredLookDir.z) >= 0.001f)
        {
            usingControllerTimer = 0.0f;
            facing = MakeDirectionCamRelative(desiredLookDir);
        }
        else if (Mathf.Abs(desiredMoveDir.x) >= 0.001f || Mathf.Abs(desiredMoveDir.z) >= 0.001f)
        {
            if (usingControllerTimer > 0.0f)
            {
                usingControllerTimer -= Time.deltaTime;
            }
            else
            {
                facing = MakeDirectionCamRelative(desiredMoveDir);
            }
        }

        facing = Vector3.RotateTowards(tf.forward, facing, rotSpeed * Time.deltaTime, 0.0f);
    }

    Vector3 MakeDirectionCamRelative (Vector3 direction)
    {
        Vector3 camFwdRelative = playCamTf.forward * direction.z;
        Vector3 camRgtRelative = playCamTf.right * direction.x;

        Vector3 camRelativeMove = camFwdRelative + camRgtRelative;
        camRelativeMove.y = 0.0f;

        return camRelativeMove;
    }

    public void TeleportPlayerTo (Vector3 destination)
    {
        tf.position = destination;
        playCam.TeleportPlayCamTo(destination);
    }
}

// Multiple Camera Angles - 30, 45, 75, 90