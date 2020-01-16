using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FDScriptAI : MonoBehaviour
{
	public Animator animator;
	public BoxCollider2D weaponRange;
	public bool isFacingRight;
	private bool firingStance = false;

	// boolean variable
	bool isShoot = false;

	// damage variable
	public int health = 100;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		animator.SetBool("isShoot", true);
		isShoot = true;
		firingStance = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		animator.SetBool("isShoot", false);
		isShoot = false;
		firingStance = false;
	}

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
	}
}
