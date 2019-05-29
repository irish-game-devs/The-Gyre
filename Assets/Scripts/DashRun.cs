using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    Idle,
    Spin,
    DashOrRun,
    Dashing,
    Running,
    CoolDown
}


//[RequireComponent(typeof(CharMotor))]
public class DashRun : MonoBehaviour
{
    private Vector3 desiredMove = Vector3.zero;

    [SerializeField]
    private float dashStrenght = 100.0f;
    [SerializeField]
    private float dashLenght = 0.2f;
    private float dashTimer;
    private Vector3 dashVelocity;
    public Vector3 dashDestination;


    [SerializeField]
    public float spinLenght = 0.2f;
    public float spinTimer = 0.2f;
    public Quaternion initialRotation;

    [SerializeField]
    private float runStrenght = 2.0f;
    [SerializeField]
    private float runLenght = 3f;
    private float runTimer;
    private Vector3 runVelocity;

    private float dashOrRunLenght = 0.3f;
    private float dashOrRunTimer = 0.3f;

    [SerializeField]
    private float coolDownLenght = 0.5f;
    private float coolDownTimer = 0.5f;

    [SerializeField]
    private float runCoolDownLenght = 0.5f;
    private float runCoolDownTimer = 0.5f;
    
    private Transform tf;
    public GameObject dashEffect;

    private Status status = Status.Idle;

    private void Awake()
    {
        // motor = GetComponent<CharMotor>();
        tf = GetComponent<Transform>();
        dashTimer = dashLenght;
        coolDownTimer = coolDownLenght;
        runTimer = runLenght;
        runCoolDownTimer = runCoolDownLenght;
        spinTimer = spinLenght;
        initialRotation = tf.rotation;
    }

    private void FixedUpdate()
    {
        if (this.status == Status.Dashing)
        {
            Instantiate(dashEffect, tf.position, Quaternion.identity);
        }
    }

    public void ReiceiveInput(Vector3 direction)
    {
        if (status == Status.Idle)
        {
            dashVelocity = direction * dashStrenght;
        }
    }

    public void SetStatus(Status status)
    {
        this.status = status;
    }

    public Status GetStatus()
    {
        return this.status;
    }

    public void SetDashDestination(Vector3 direction)
    {
        dashDestination = direction * dashStrenght;
    }

    public Status StatusUpdate(Vector3 desiredMove)
    {
        this.desiredMove = desiredMove;
        Status status = this.status;
        KeyCode dashCode = KeyCode.L;
        switch (status)
        {
            case Status.Idle:
                if (Input.GetKeyDown(dashCode))
                {
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    {
                        status = Status.DashOrRun;
                    }
                    else
                    {
                        status = Status.Spin;
                        spinTimer = spinLenght;

                        initialRotation = Quaternion.Euler(0, 360, 0) * tf.rotation;
                    }
                }
                break;
            case Status.Spin:
                if (spinTimer >= 0)
                {
                    spinTimer -= Time.deltaTime;
                }
                else
                {
                    spinTimer = spinLenght;
                    status = Status.Idle;
                }
                break;
            case Status.DashOrRun:
                dashOrRunTimer -= Time.deltaTime;

                if (dashOrRunTimer <= 0f && Input.GetKey(dashCode))
                {
                    status = Status.Running;
                    dashOrRunTimer = dashOrRunLenght;
                }
                else if (Input.GetKeyUp(dashCode))
                {
                    Debug.Log(Status.Dashing);
                    status = Status.Dashing;
                    dashOrRunTimer = dashOrRunLenght;

                    Instantiate(dashEffect, tf.position, Quaternion.identity);
                    SetDashDestination(desiredMove);
                }
                break;
            case Status.Dashing:
                if (dashTimer > 0)
                {
                    Instantiate(dashEffect, tf.position, Quaternion.identity);
                    dashTimer -= Time.deltaTime;
                }
                if (dashTimer <= 0)
                {
                    status = Status.CoolDown;
                    dashTimer = dashLenght;
                }
                break;
            case Status.Running:
                if (Input.GetKey(dashCode))
                {
                    if (runTimer > 0)
                    {
                        runTimer -= Time.deltaTime;
                    }
                }
                if (this.runTimer <= 0f || Input.GetKeyUp(dashCode))
                {
                    status = Status.CoolDown;
                    runTimer = runLenght;
                }
                break;
            case Status.CoolDown:
                if (this.coolDownTimer > 0)
                    coolDownTimer -= Time.deltaTime;
                else
                {
                    coolDownTimer = coolDownLenght;
                    status = Status.Idle;
                }
                if (this.runCoolDownTimer >= 0)
                    runCoolDownTimer -= Time.deltaTime;
                else
                {
                    runCoolDownTimer = runCoolDownLenght;
                    status = Status.Idle;
                }
                break;
        }
        this.status = status;
        return status;
    }

    public Vector3 DashModifier(Vector3 desiredMove)//, float deltaT)
    {
        Vector3 modifier = desiredMove;
        switch (status)
        {
            case Status.Dashing:
                modifier += dashDestination * Time.deltaTime;
                break;
            case Status.Running:
                modifier *= runStrenght;
                break;
        }
        return modifier;
    }
}



