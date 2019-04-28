using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private Transform   tf;
    private Rigidbody   rb;

    private Transform   playerTF;
    private Vector3     cameraOffsetFromPlayer;

    private Vector3     targetPosition;

    [SerializeField]
    private float       moveSpeed = 1.0f;

    private void Awake()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTF = GameObject.Find("Player").GetComponent<Transform>();

        cameraOffsetFromPlayer = tf.position - playerTF.position;
    }

    private void FixedUpdate()
    {
        targetPosition = playerTF.position + playerTF.forward + cameraOffsetFromPlayer;

        Vector3 desiredMove = Vector3.Lerp(rb.position, targetPosition, moveSpeed * Time.deltaTime);

        rb.MovePosition(desiredMove);
    }
}
