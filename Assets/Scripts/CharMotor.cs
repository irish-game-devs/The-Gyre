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


    private void Awake()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        dash = GetComponent<DashRun>();
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

    public void ReceiveInput (Vector3 desiredMove, Vector3 facing)
    {
        this.desiredMove = desiredMove;
        this.facing = facing;
    }
}
