using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class TestPlayerController : MonoBehaviour {
    public float maxSpeed = 10f;
    public float speed;

    public float jumpForce = 10f;
    public float gravity = 5f;

    private Rigidbody rb;
    private GameController controller;
    private float distToGround;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GameController.Singleton;
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f) * speed;
        movement.y = rb.velocity.y;
        rb.velocity = movement;
   

        Vector3 clampedVelocity = rb.velocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxSpeed, maxSpeed);
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -maxSpeed, maxSpeed);
        clampedVelocity.z = 0;
        rb.velocity = clampedVelocity;

        if (IsGrounded())
        {
            if (controller.isJump())
            {
                jump();
            }
        } else
        {
            rb.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.VelocityChange);
        }

    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f - 1.5f);
     }

    private void jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

  

}
