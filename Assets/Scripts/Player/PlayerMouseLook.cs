using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float cameraRotationExtremes = 90f;
    [SerializeField] private Transform playerCamera;

    private float _mouseY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks mouse cursor to the window and hides it
    }

    /// <summary>
    /// Rotates the player's body left and right
    /// </summary>
    /// <param name="context"></param>
    public void MouseXMove(InputAction.CallbackContext context)
    {
        transform.Rotate(
            Vector3.up,
            context.ReadValue<float>() * Time.deltaTime * mouseSensitivity
        );
    }

    /// <summary>
    /// Rotates the player's camera up and down
    /// </summary>
    /// <param name="context"></param>
    public void MouseYMove(InputAction.CallbackContext context)
    {
        _mouseY = Mathf.Clamp(
            _mouseY - context.ReadValue<float>() * Time.deltaTime * mouseSensitivity,
            -cameraRotationExtremes, cameraRotationExtremes);

        playerCamera.localRotation = Quaternion.Euler(_mouseY, 0f, 0f);
    }
}