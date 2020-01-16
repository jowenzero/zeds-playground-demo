using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAI : MonoBehaviour
{
	public Transform firePoint;
	public GameObject bulletPrefab;
	public BoxCollider2D weaponRange;
	public float shotsPerSecond = 1.0f;
	public float fireDelay = 0f;

	private void OnTriggerStay2D(Collider2D collision)
	{
		float fireRate = 1.0f / shotsPerSecond;
		InvokeRepeating("Shoot", fireDelay, fireRate);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		CancelInvoke("Shoot");
	}

	void Shoot()
	{
		// spawn bullet at fire point position
		Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
	}
}
