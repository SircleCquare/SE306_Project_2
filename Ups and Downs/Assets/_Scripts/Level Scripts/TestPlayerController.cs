using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class TestPlayerController : MonoBehaviour {
    public float speed;
    public float tilt;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        rb.velocity = movement * speed;
    }
}
