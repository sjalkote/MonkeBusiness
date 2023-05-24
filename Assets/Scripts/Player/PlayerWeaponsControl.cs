using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsControl : MonoBehaviour
{
    public static Action ShootInput;
    public static Action ReloadInput;
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ShootInput?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadInput?.Invoke();
        }
    }
}
