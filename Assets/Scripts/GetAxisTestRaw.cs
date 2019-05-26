using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAxisTestRaw : MonoBehaviour
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
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        moveInput.y = 0.0f;

        moveInput *= Time.deltaTime;
    }
}
