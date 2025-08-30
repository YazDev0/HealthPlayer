using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 0.2f;

    [Header("Sound Settings")]
    public AudioClip shootSound;   
    public float soundVolume = 1f;

    private void Start()
    {
        StartCoroutine(AutoShoot());
    }

    private System.Collections.IEnumerator AutoShoot()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;

        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, bulletSpawnPoint.position, soundVolume);
        }
    }
}
