using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMotor : MonoBehaviour
{
    private const string horInputTwo = "HorizontalTwo";
    private const string verInputTwo = "VerticalTwo";

    [SerializeField]
    private float moveSpeed = 1.0f;

    private Transform tf;
    private Rigidbody rb;

    private Vector3 desiredDir = Vector3.zero;

    private void Awake()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(desiredDir.x) >= 0.01f || Mathf.Abs(desiredDir.z) >= 0.01f)
        {
            rb.MoveRotation(Quaternion.LookRotation(desiredDir));

            Vector3 desiredMove = desiredDir * moveSpeed * Time.deltaTime;

            rb.MovePosition(rb.position + desiredMove);
        }
    }

    // Update is called once per frame
    void Update()
    {
        desiredDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        
        Debug.Log("Second stick hor sez wot? " + Input.GetAxis(horInputTwo));
        Debug.Log("Second stick ver sez wot? " + Input.GetAxis(verInputTwo));        
    }
}
