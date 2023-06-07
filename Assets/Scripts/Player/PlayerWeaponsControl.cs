using System;
using UnityEngine;

public class PlayerWeaponsControl : MonoBehaviour
{
    public static Action ShootInput;
    public static Action ReloadInput;

    private void Update()
    {
        if (Input.GetMouseButton(0)) ShootInput?.Invoke();

        if (Input.GetKeyDown(KeyCode.R)) ReloadInput?.Invoke();
    }
}