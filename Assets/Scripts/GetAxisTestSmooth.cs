using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAxisTestSmooth : MonoBehaviour
{
    private Rigidbody rb;

    Vector3 moveInput = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        moveInput.y = 0.0f;

        moveInput *= Time.deltaTime;
    }

}
