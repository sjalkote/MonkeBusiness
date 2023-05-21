using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private TrailRenderer BulletTrail;
    
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
        if (gunData.currentAmmo <= 0) return;
        if (!CanShoot()) return;
        if (Physics.Raycast(muzzle.position, -transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
        {
            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();

            TrailRenderer trail = Instantiate(BulletTrail, muzzle.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hitInfo));
                    
            damageable?.Damage(gunData.damage);
        }
                
        gunData.currentAmmo--;
        timeSinceLastShot = 0;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = Hit.point;
        
        Destroy(Trail.gameObject, Trail.time);
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        
        Debug.DrawRay(muzzle.position, -muzzle.forward * gunData.maxDistance);
    }
}
