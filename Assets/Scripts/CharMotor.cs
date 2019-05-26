using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharMotor : MonoBehaviour
{    
    private Transform tf;
    private Rigidbody rb;

    private Vector3 desiredMove = Vector3.zero;
    private Vector3 facing = Vector3.zero;

    private void Awake()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + desiredMove);

        if (facing.magnitude > 0.001f) rb.MoveRotation(Quaternion.LookRotation(facing));

        rb.angularVelocity = Vector3.zero;
    }

    public void ReceiveInput (Vector3 desiredMove, Vector3 facing)
    {
        this.desiredMove = desiredMove;
        this.facing = facing;
    }
}
