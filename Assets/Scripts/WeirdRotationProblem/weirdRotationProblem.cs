using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weirdRotationProblem : MonoBehaviour
{
    public float rotateTime     = 3.0f; // seconds
    public float waitTime       = 1.0f; // seconds
    public float rotationMax    = 360.0f; // degrees
    public float timer          = 0.0f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // rotate the object
        if (timer < rotateTime)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * Time.deltaTime * rotationMax / rotateTime));
        }
    }

    void Update()
    {
        // increment timer
        timer += Time.deltaTime;
        // reset the timer after it goes past total time
        while (timer > rotateTime + waitTime)
        {
            timer -= rotateTime + waitTime;
        }
    }
}
