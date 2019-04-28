using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerHudFollower : MonoBehaviour
{
    private Transform tfPlayer;
    private Rigidbody rb;

    private void Awake()
    {
        tfPlayer = GameObject.Find("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(tfPlayer.position);
    }
}
