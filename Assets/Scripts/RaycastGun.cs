using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;
    
    private float timeSinceLastShot;
    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Start()
    {
        PlayerWeaponsControl.ShootInput += Shoot;
        PlayerWeaponsControl.ReloadInput += StartReload;
        
        gunData.currentAmmo = gunData.magSize;
    }

    public void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);
        
        gunData.currentAmmo = gunData.magSize;
        
        gunData.reloading = false;
    }

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(muzzle.position, -transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    Debug.Log(hitInfo.transform.name);
                    Debug.Log(hitInfo.transform.GetComponent<IDamageable>());
                    Debug.Log("Shot");
                    damageable?.Damage(gunData.damage);
                }
                
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        
        Debug.DrawRay(muzzle.position, -muzzle.forward * gunData.maxDistance);
    }

    private void OnGunShot()
    {
        throw new NotImplementedException();
    }
}
