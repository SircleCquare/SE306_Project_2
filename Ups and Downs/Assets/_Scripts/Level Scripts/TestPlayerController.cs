using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class TestPlayerController : MonoBehaviour {
    public float maxSpeed = 10f;
    public float speed;

    public float jumpForce = 10f;
    public float gravity = 5f;
    public float airtime = 1f;

    public float deadzone = 0.2f;

    private Rigidbody rb;
    private Animator animator;
    private GameController controller;
    private float distToGround;

    private float airTimeCount;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GameController.Singleton;
        distToGround = GetComponent<Collider>().bounds.extents.y;
        animator = GetComponent<Animator>();
    }
    

}
