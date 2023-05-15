/*
 * Caleb Kahn
 * Assignment 5B
 * Moves player based on gravity and user input
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public Vector3 velocity;
    public float gravity = -9.8f;
    public float gravityMultiplier = 2f;

    public Transform groundCheck;
    public float groundDistence = .4f;
    public LayerMask groundMask;
    public bool isGrounded;

    public float jumpHeight = 3f;

    void Awake()
    {
        gravity *= gravityMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundMask);
        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        if(Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
