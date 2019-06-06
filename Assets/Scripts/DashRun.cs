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
    private KeyCode dashCode = KeyCode.LeftShift;

    private Vector3 desiredMove = Vector3.zero;

    [SerializeField]
    private float dashStrength = 100.0f;
    [SerializeField]
    private float dashTimerLength = 0.2f;
    private float dashTimer;
    [SerializeField]
    private Vector3 dashVelocity;
    private Vector3 dashDestination;
    [SerializeField]
    private GameObject dashEffect;

    [SerializeField]
    private float spinTimerLength = 0.2f;
    private float spinTimer = 0.2f;
    private Quaternion targetRotation;

    [SerializeField]
    private float runStrength = 2.0f;
    [SerializeField]
    private float runTimerLength = 3f;
    private float runTimer;
    private Vector3 runVelocity;

    [SerializeField]
    private float dashOrRunTimerLength = 0.3f;
    private float dashOrRunTimer = 0.3f;

    [SerializeField]
    private float coolDownTimerLength = 0.5f;
    private float coolDownTimer = 0.5f;
    
    private Transform tf;

    private Status status = Status.Idle;

    private void Awake()
    {
        // motor = GetComponent<CharMotor>();
        tf = GetComponent<Transform>();
        dashTimer = dashTimerLength;
        coolDownTimer = coolDownTimerLength;
        runTimer = runTimerLength;
        spinTimer = spinTimerLength;
        targetRotation = tf.rotation;
    }

    private void FixedUpdate()
    {
        if (this.status == Status.Dashing)
        {
            Debug.Log(dashEffect);
            Instantiate(dashEffect, tf.position, Quaternion.identity);
        }
    }

    public void ReiceiveInput(Vector3 direction)
    {
        if (status == Status.Idle)
        {
            dashVelocity = direction * dashStrength;
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
        dashDestination = direction * dashStrength;
    }

    public Status StatusUpdate(Vector3 desiredMove)
    {
        this.desiredMove = desiredMove;
        Status status = this.status;
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
                        spinTimer = spinTimerLength;

                        targetRotation = Quaternion.Euler(0, 360, 0) * tf.rotation;
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
                    spinTimer = spinTimerLength;
                    status = Status.Idle;
                }
                break;
            case Status.DashOrRun:
                dashOrRunTimer -= Time.deltaTime;

                if (dashOrRunTimer <= 0f && Input.GetKey(dashCode))
                {
                    status = Status.Running;
                    dashOrRunTimer = dashOrRunTimerLength;
                }
                else if (Input.GetKeyUp(dashCode))
                {
                    status = Status.Dashing;
                    dashOrRunTimer = dashOrRunTimerLength;

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
                    dashTimer = dashTimerLength;
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
                    runTimer = runTimerLength;
                }
                break;
            case Status.CoolDown:
                if (this.coolDownTimer > 0)
                    coolDownTimer -= Time.deltaTime;
                else
                {
                    coolDownTimer = coolDownTimerLength;
                    status = Status.Idle;
                }
                break;
        }
        this.status = status;
        return status;
    }

    public Vector3 MovementModifier(Vector3 desiredMove)
    {
        Vector3 modifier = desiredMove;
        switch (status)
        {
            case Status.Dashing:
                modifier += dashDestination * Time.deltaTime;
                break;
            case Status.Running:
                modifier *= runStrength;
                break;
        }
        return modifier;
    }


    //GETTERS

    public float GetSpinLength()
    {
        return spinTimerLength;
    }

    public Quaternion GetInitialRotation()
    {
        return targetRotation;
    }

    
}



