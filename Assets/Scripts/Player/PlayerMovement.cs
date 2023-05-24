using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    
    private Vector2 _moveInput;
    private CharacterController _characterController;

    private void Awake() { _characterController = GetComponent<CharacterController>(); }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
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