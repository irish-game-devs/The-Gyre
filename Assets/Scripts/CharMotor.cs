using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharMotor : MonoBehaviour
{    
    private Transform tf;
    private Rigidbody rb;
    public DashRun dash;

    private Vector3 desiredMove = Vector3.zero;
    private Vector3 facing = Vector3.zero;

    // PLAYERINPUTS VARIABLES
    private float usingControllerTimer = 0.0f;
    private float usingControllerMax = 1.0f;
    [SerializeField]
    public float moveSpeed = 1.0f;
    [SerializeField]
    private float rotSpeed = 8.0f;
    private PlayerCam playCam;

    private Vector3 desiredMoveDir = Vector3.zero;
    private Vector3 desiredLookDir = Vector3.zero;
    private Vector3 mouseMovement = Vector3.zero;


    private void Awake()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        dash = GetComponent<DashRun>();
    }

    private void Start()
    {
        playCam = GameObject.Find("Main Camera").GetComponent<PlayerCam>();
    }


        private void FixedUpdate()
    {
        Vector3 move = desiredMove;

        move = dash.DashModifier(move);
        Vector3 target_pos = rb.position + move;

        if (dash.GetStatus() != Status.Spin)
            rb.MovePosition(target_pos);
        if (dash.GetStatus() == Status.Spin)
        {
            float delta = (360f * Time.deltaTime) / dash.spinLenght;
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * delta);
            Quaternion desiredRotation = deltaRotation * rb.rotation;
            if (Quaternion.Angle(desiredRotation, dash.initialRotation) > 0.1)
                rb.MoveRotation(desiredRotation);
            else
            {
                //TEMPORARY FIX TO ROTATION BUG
                rb.MoveRotation(dash.initialRotation);
                dash.SetStatus(Status.Idle);
            }
        }
        else
        {
            if (facing.magnitude > 0.001f) rb.MoveRotation(Quaternion.LookRotation(facing));

        }
        
        rb.angularVelocity = Vector3.zero;
    }

    public void ReceiveInput (Vector3 desiredMove)
    {
        this.desiredMove = desiredMove;
        dash.StatusUpdate(desiredMove);
    }


    //was PlayerInputs.HandleFacing
    public void SetFacing(Vector3 desiredMoveDir, Vector3 desiredLookDir, Vector3 mouseMovement)
    {
        if (Mathf.Abs(mouseMovement.x) >= 0.001f || Mathf.Abs(mouseMovement.z) >= 0.001f)
        {
            usingControllerTimer = usingControllerMax;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(playCam.GetTfPosition(), tf.position);

            Vector3 v3 = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 desiredLookDirMouse = v3 - tf.position;

            facing = playCam.MakeDirectionCamRelative(desiredLookDirMouse);
        }
        else if (Mathf.Abs(desiredLookDir.x) >= 0.001f || Mathf.Abs(desiredLookDir.z) >= 0.001f)
        {
            usingControllerTimer = 0.0f;
            facing = playCam.MakeDirectionCamRelative(desiredLookDir);
        }
        else if (Mathf.Abs(desiredMoveDir.x) >= 0.001f || Mathf.Abs(desiredMoveDir.z) >= 0.001f)
        {
            if (usingControllerTimer > 0.0f)
            {
                usingControllerTimer -= Time.deltaTime;
            }
            else
            {
                facing = playCam.MakeDirectionCamRelative(desiredMoveDir);
            }
        }

        facing = Vector3.RotateTowards(tf.forward, facing, rotSpeed * Time.deltaTime, 0.0f);
    }

    public void TeleportPlayerTo(Vector3 destination)
    {
        tf.position = destination;
        playCam.TeleportPlayCamTo(destination);
    }

    public void ReceiveDirections(Vector3 desiredMoveDir, Vector3 desiredLookDir, Vector3 mouseMovement)
    {
        this.desiredMoveDir = desiredMoveDir;
        this.desiredLookDir = desiredLookDir;
        this.mouseMovement = mouseMovement;
    }
}
