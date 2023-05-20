using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerBody;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    public float jumpForce = 5f;
    public float speed = 8f;
    public Object groundObject;

    private Vector3 velocity;
    bool isGrounded;

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        // InputActions stuff
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // void FixedUpdate()
    // {
    //     Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
    //     playerBody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    // }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))) * speed;

        playerBody.velocity = new Vector3(move.x, playerBody.velocity.y, move.z);
    }
    
        
    #region Input Handling
    public void Jump(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jump " + ctx.phase);
        if (!isGrounded)
            return;
        playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    #endregion

    #region Collision Detection
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
    #endregion
}