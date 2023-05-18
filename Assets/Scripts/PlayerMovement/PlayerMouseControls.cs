using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseControls : MonoBehaviour
{
    public Vector2 sensitivity;
    private Vector2 rotation;
    public enum RotationDirection
    {
        None,
        Horizontal = (1 << 0),
        Vertical = (1 << 1),
    }

    public RotationDirection rotationDirections;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );
        
        Vector2 wantedVelocity = mouse * sensitivity;

        if ((rotationDirections & RotationDirection.Vertical) == 0)
        {
            wantedVelocity.y = 0;
        }
        
        if ((rotationDirections & RotationDirection.Horizontal) == 0)
        {
            wantedVelocity.x = 0;
        }
        
        rotation += wantedVelocity * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);

        transform.localEulerAngles = new Vector3(rotation.y, rotation.x, 0);
    }
}
