using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidSword : Weapon
{
    private Transform tf;

    private Vector3 animationDistance = new Vector3(0f, 0.5f, 0f);
    private float animationLength = 1.3f;

    private Vector3 animStart;
    private Vector3 animEnd;

    private float animationTimer;
    private float animationTimerLength = 0.2f;
    private float startTime;

    private float attackSpeed;

    private bool attacking = false;

    //video variables
    private Animator animator;
    [SerializeField]
    private GameObject hitEffect;

    private void Awake()
    {
        tf = GetComponent<Transform>();
        animationTimer = animationTimerLength;
        attackSpeed = animationLength / animationTimerLength;

        //video code 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //should come from PlayerInputs
        if (Input.GetMouseButtonDown(0) && animationTimer == animationTimerLength)
            StartAttack();
        else if (attacking && animationTimer <= 0)
        {
            animationTimer = animationTimerLength;
            attacking = false;
        }
        else if (attacking && animationTimer > 0)
        {
            float distCovered = (Time.time - startTime) * attackSpeed;

            // Fraction of journey completed = current distance divided by total distance.
            float deltaAnimation = distCovered / animationLength;

            // Set our position as a fraction of the distance between the markers.
            Vector3 updatedStart = tf.position;
            updatedStart.y = animStart.y;
            Vector3 updatedEnd = tf.position;
            updatedEnd.y = animEnd.y;

            tf.position = Vector3.Lerp(updatedStart, updatedEnd, deltaAnimation);

            animationTimer -= Time.deltaTime;

        }


        // from video
        if (Input.GetMouseButtonDown(1))
            PerformAttack();
    }

    public void StartAttack()
    {
        startTime = Time.time;
        animStart = tf.position + animationDistance;
        animEnd = tf.position;
        tf.position = animEnd;
        attacking = true;

    }

    public void PerformAttack()
    {
        //res: https://www.youtube.com/watch?v=HrNebvxSUsU
        //notes from video: the animation are gonna be handled by the weapon and 
        // not by the hand. This is because the animation of each weapon my be different
        // and it might also change weather it's in idle, fight stance etc
        // so each weapon is gonna have it's own animator controller and the animation is gonna
        // be confined in the weapon. The hand is gonna be there just kinda like a bone to attach stuff 
        // to (expect I don't have all he has yet probably). 
        // In any case since we will probably drive through the character animation in the end in our actual
        //build

        //on KidSword prefab setting Is Trigger = true prevents the sword to affect the physics 
        //(doesn'thit the walls/cars, doesn't push stuff around).
        //I guess it is still possible to catch the collision and inflict damage if is an enemy etc.
        animator.SetTrigger("Base_Attack");

    }

    //engine method
    void OnTriggerEnter(Collider other)
    {
        //video note: the reason why the collider is off while the weapon goes back to normal position
        //with the animation is that otherwise OnTriggerEnter would fire again
        if (other.name != "Player")
        {
            Debug.Log("Hit: " + other.name);
            Vector3 collisionPos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(tf.position);
            Instantiate(hitEffect, collisionPos, Quaternion.identity);
        }

        if (other.tag == "Enemy")
        {
            //do stuff inflict damage
            Destroy(other.gameObject);
        }
    }
}
