using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody playerBody;

    public float jumpForce = 10f;
    public float speed = 5f;
    public Object groundObject;

    private Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))) * speed;

        playerBody.velocity = new Vector3(move.x, playerBody.velocity.y, move.z);
        
        if (Input.GetButton("Jump"))
        {
            if (!isGrounded)
            {
                return;
            }
            playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(gameObject.tag))
        {
            isGrounded = true;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(gameObject.tag))
        {
            isGrounded = false;
        }
    }
}