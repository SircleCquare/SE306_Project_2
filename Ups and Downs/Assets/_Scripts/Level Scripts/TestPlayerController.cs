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
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (moveHorizontal < deadzone && -deadzone < moveHorizontal)
        {
            moveHorizontal = 0f;
        } else
        {
            moveHorizontal = (moveHorizontal > 0) ? 1 : -1;
        }

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
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                airTimeCount = airtime;
            }
        } else
        {
            if (controller.isJump() && airTimeCount > 0)
            {
                airTimeCount -= Time.fixedDeltaTime;
            } else
            {
                rb.AddForce(Vector3.down * gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
               
        }
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f - 1.5f);
     }

}
