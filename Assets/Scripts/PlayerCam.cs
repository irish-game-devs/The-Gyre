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

    public void TeleportPlayCamTo(Vector3 destination)
    {
        tf.position = destination + playerTF.forward + cameraOffsetFromPlayer;
    }

    public Vector3 MakeDirectionCamRelative(Vector3 direction)
    {
        Vector3 camFwdRelative = tf.forward * direction.z;
        Vector3 camRgtRelative = tf.right * direction.x;

        Vector3 camRelativeMove = camFwdRelative + camRgtRelative;
        camRelativeMove.y = 0.0f;

        return camRelativeMove;
    }

    public Vector3 GetTfPosition()
    {
        return tf.position;
    }
}
