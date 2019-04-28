using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharMotor))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float rotSpeed = 8.0f;

    private Transform tf;

    private const string horInputTwo = "HorizontalTwo";
    private const string verInputTwo = "VerticalTwo";

    private Vector3 facing = Vector3.zero;

    private float   usingControllerTimer = 0.0f;
    private float   usingControllerMax = 1.0f;

    private CharMotor motor;

    private void Awake()
    {
        tf  = GetComponent<Transform>();
        motor = GetComponent<CharMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredMoveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        Vector3 desiredLookDir = new Vector3(Input.GetAxisRaw(horInputTwo), 0.0f, Input.GetAxisRaw(verInputTwo));

        Vector3 mouseMovement = new Vector3(Input.GetAxisRaw("Mouse X"), 0.0f, Input.GetAxisRaw("Mouse Y"));

        HandleFacing(desiredMoveDir, desiredLookDir, mouseMovement);

        Vector3 desiredMove = desiredMoveDir * moveSpeed * Time.deltaTime;

        motor.ReceiveInput(desiredMove, facing);
    }

    void HandleFacing(Vector3 desiredMoveDir, Vector3 desiredLookDir, Vector3 mouseMovement)
    {
        if (Mathf.Abs(mouseMovement.x) >= 0.001f || Mathf.Abs(mouseMovement.z) >= 0.001f)
        {
            usingControllerTimer = usingControllerMax;
            Vector3 mousePos = Input.mousePosition;
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 128.0f))
            {
                Vector3 desiredLookDirMouse = raycastHit.point - tf.position;
                desiredLookDirMouse = new Vector3(desiredLookDirMouse.x, 0.0f, desiredLookDirMouse.z);
                facing = desiredLookDirMouse;
            }
        }
        else if (Mathf.Abs(desiredLookDir.x) >= 0.001f || Mathf.Abs(desiredLookDir.z) >= 0.001f)
        {
            usingControllerTimer = 0.0f;
            facing = desiredLookDir;
        }
        else if (Mathf.Abs(desiredMoveDir.x) >= 0.001f || Mathf.Abs(desiredMoveDir.z) >= 0.001f)
        {
            if (usingControllerTimer > 0.0f)
            {
                usingControllerTimer -= Time.deltaTime;
            }
            else
            {
                facing = desiredMoveDir; // Possibly rotateTowards this point with high speed, just to add a little smoothness
            }
        }

        facing = Vector3.RotateTowards(tf.forward, facing, rotSpeed * Time.deltaTime, 0.0f);
    }
}
