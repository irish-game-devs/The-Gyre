using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharMotor))]
public class PlayerInput : MonoBehaviour
{
    private Transform tf;

    private const string horInputTwo = "HorizontalTwo";
    private const string verInputTwo = "VerticalTwo";

    private CharMotor motor;
    private PlayerCam playCam;

    private void Awake()
    {
        tf  = GetComponent<Transform>();
        motor = GetComponent<CharMotor>();
        playCam = GameObject.Find("Main Camera").GetComponent<PlayerCam>();
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 desiredMoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        Vector3 desiredLookDir = new Vector3(Input.GetAxisRaw(horInputTwo), 0.0f, Input.GetAxisRaw(verInputTwo));

        Vector3 mouseMovement = new Vector3(Input.GetAxisRaw("Mouse X"), 0.0f, Input.GetAxisRaw("Mouse Y"));

        motor.SetFacing(desiredMoveDir, desiredLookDir, mouseMovement);

        Vector3 desiredMove = Vector3.zero;

        if(Mathf.Abs(desiredMoveDir.x) >= 0.1f || Mathf.Abs(desiredMoveDir.z) >= 0.1f)
        {
            desiredMove = playCam.MakeDirectionCamRelative(desiredMoveDir).normalized * motor.moveSpeed * Time.deltaTime;
            // desiredMove = desiredMoveDir.normalized * moveSpeed * Time.deltaTime;
        }

        motor.ReceiveInput(desiredMove);
    }

}

// Multiple Camera Angles - 30, 45, 75, 90

