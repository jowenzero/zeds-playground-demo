using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenDummyScript : MonoBehaviour
{
	// damage variable
	public int health = 60;

	// damage function
	public void TakeDamage(int damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Die();
		}
	}

	// despawn function
	void Die()
	{
		Destroy(gameObject);
		FindObjectOfType<TargetTrialScript>().targetKill++;
	}
}
