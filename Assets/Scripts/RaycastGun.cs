using System.Collections;
using UnityEngine;

public class RaycastGun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private TrailRenderer BulletTrail;

    private float timeSinceLastShot;

    private void Start()
    {
        PlayerWeaponsControl.ShootInput += Shoot;
        PlayerWeaponsControl.ReloadInput += StartReload;

        gunData.currentAmmo = gunData.magSize;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(muzzle.position, -muzzle.forward * gunData.maxDistance);
    }

    private bool CanShoot()
    {
        return !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    }

    public void StartReload()
    {
        if (!gunData.reloading) StartCoroutine(Reload());
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
        if (Physics.Raycast(muzzle.position, -transform.forward, out var hitInfo, gunData.maxDistance))
        {
            var damageable = hitInfo.transform.GetComponent<IDamageable>();

            var trail = Instantiate(BulletTrail, muzzle.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hitInfo));
            AudioManager.instance.PlayOneShot(FMODEvents.instance.pistolShootSound, transform.position);

            damageable?.Damage(gunData.damage);
        }

        gunData.currentAmmo--;
        timeSinceLastShot = 0;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        var startPosition = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }

        Trail.transform.position = Hit.point;

        Destroy(Trail.gameObject, Trail.time);
    }
}