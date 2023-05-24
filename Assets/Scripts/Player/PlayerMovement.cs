using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float gravityMultiplier = 1f;
    private CharacterController _characterController;

    private Vector2 _moveInput;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyExternalForces();
        ApplyMovement();
        // Debug.Log(_characterController.isGrounded);
    }

    private void ApplyExternalForces()
    {
        _characterController.Move(Physics.gravity * (gravityMultiplier * Time.deltaTime));
    }

    private void ApplyMovement()
    {
        var playerTransform = transform;

        var movementDirection = playerTransform.right * _moveInput.x + playerTransform.forward * _moveInput.y;
        _characterController.Move(movementDirection * (movementSpeed * Time.deltaTime));
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
}