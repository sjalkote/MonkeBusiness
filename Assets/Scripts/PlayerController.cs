using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Throw a warning when someone tries to remove the CharacterController component
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [SerializeField] private float speed;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _characterController.Move(_direction * (speed * Time.deltaTime));
    }

    // This is called from the InputSystem, by selecting the Move method for the WASD keys
    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }
}