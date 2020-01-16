using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shotsPerSecond = 1.0f;
    public float fireDelay = 0f;

    // Update is called once per frame
    void Update()
    {
        float fireRate = 1.0f / shotsPerSecond;

        // firing function
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Shoot", fireDelay, fireRate);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Shoot");
        }
    }

    void Shoot()
    {
        // spawn bullet at fire point position
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
