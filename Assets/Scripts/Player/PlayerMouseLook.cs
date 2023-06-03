using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float cameraRotationExtreme = 90f;
    [SerializeField] private Transform playerCamera;

    private float _mouseY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks mouse cursor to the window and hides it
    }
    
    public void MouseMove(InputAction.CallbackContext context)
    {
        var mouseDelta = context.ReadValue<Vector2>();
        _mouseY = Mathf.Clamp(
            _mouseY - mouseDelta.y * Time.deltaTime * mouseSensitivity,
            -cameraRotationExtreme, cameraRotationExtreme);

        transform.Rotate(
            Vector3.up,
            mouseDelta.x * Time.deltaTime * mouseSensitivity
        );
        playerCamera.localRotation = Quaternion.Euler(_mouseY, 0f, 0f);
    }
}