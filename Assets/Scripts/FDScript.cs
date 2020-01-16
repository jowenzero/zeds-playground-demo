using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FDScript : MonoBehaviour
{
	public CharacterController2D controller;
	public Animator animator;
    public bool isFacingRight;
    private bool firingStance = false;

    // movement speed variable
    public float runSpeed = 30f;
    float horizontalMove = 0f;
    float verticalMove = 0f;

    // constant speed variable
    const float IDLE = 0f;
    const float SLOW = 20f;
    const float NORMAL = 30f;
    const float FAST = 40f;
    const float HYPERSPEED = 1500f;

    // boolean variable
    bool isShoot = false;

    // damage variable
    public int health = 100;

    // Update is called once per frame
    void Update()
	{
		// calculated movement speed variable
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;

        // speed tracker variable
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		// firing function
		if (Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetBool("isShoot", true);
			isShoot = true;
			runSpeed = SLOW;
            firingStance = true;
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			animator.SetBool("isShoot", false);
			isShoot = false;
			runSpeed = NORMAL;
            firingStance = false;
        }
	}

	void FixedUpdate()
	{
		// movement function
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, false, true, isFacingRight, firingStance);
		controller.Move(verticalMove * Time.fixedDeltaTime, false, false, false, isFacingRight, firingStance);
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