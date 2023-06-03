using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    public float drag = 3;
    public float slowWalkMult = 0.6f;
    public float jumpPower = 20f;
    public float gravityMult = 4f;

    public Vector3 velocity = Vector3.zero;
    private CharacterController _characterController;
    private bool _isSlowWalking;
    private Vector2 _moveInput;

    public void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Update()
    {
        var playerTransform = transform;
        velocity += (playerTransform.right * _moveInput.x + playerTransform.forward * _moveInput.y) * (
            moveSpeed * (_isSlowWalking ? slowWalkMult : 1f)
        );

        if (_characterController.isGrounded && velocity.y < 0f)
            velocity.y = 0f;

        _characterController.Move(velocity * Time.deltaTime);
    }

    public void FixedUpdate()
    {
        velocity += Physics.gravity * (Time.fixedDeltaTime * gravityMult);

        velocity *= Mathf.Clamp01(1.0f - drag * Time.fixedDeltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_characterController.isGrounded) velocity.y = jumpPower;
    }

    public void SlowWalk(InputAction.CallbackContext context)
    {
        _isSlowWalking = context.ReadValueAsButton();
    }
}