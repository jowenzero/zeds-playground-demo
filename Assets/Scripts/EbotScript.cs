using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EbotScript : MonoBehaviour
{
	public CharacterController2D controller;
	public Animator animator;
    public bool isFacingRight;

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
	bool isCloak = false;
	bool isSprint = false;
	bool isStrafe = false;

	// blink cooldown
	bool blinkReady = true;
	float blinkTimer = 0.5f;

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

		// function only works when not stunned
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("EbotStun1") &&
			!animator.GetCurrentAnimatorStateInfo(0).IsName("EbotStun2") &&
			!animator.GetCurrentAnimatorStateInfo(0).IsName("EbotStun3"))
		{
			// stun function
			if (Input.GetKeyDown(KeyCode.Q))
			{
				animator.SetTrigger("isStun");
			}

			// cloak function
			if (Input.GetKeyDown(KeyCode.F) && !isCloak)
			{
				animator.SetBool("isCloak", true);
				isCloak = true;
			}
			else if (Input.GetKeyDown(KeyCode.F) && isCloak)
			{
				animator.SetBool("isCloak", false);
				isCloak = false;
			}

			// sprint function
			if (Input.GetKey(KeyCode.LeftShift) && !isCloak)
			{
				animator.SetBool("isSprint", true);
				isSprint = true;
				runSpeed = FAST;
			}
			else if (Input.GetKeyUp(KeyCode.LeftShift) && !isCloak)
			{
				animator.SetBool("isSprint", false);
				isSprint = false;
				runSpeed = NORMAL;
			}

			// strafe function
			if (Input.GetKey(KeyCode.LeftControl))
			{
				animator.SetBool("isStrafe", true);
				isStrafe = true;
				runSpeed = SLOW;
			}
			else if (Input.GetKeyUp(KeyCode.LeftControl))
			{
				animator.SetBool("isStrafe", false);
				isStrafe = false;
				runSpeed = NORMAL;
			}

			// blink function
			if (Input.GetKeyDown(KeyCode.Space) && !isCloak && blinkReady)
			{
				animator.SetBool("isBlink", true);
				blinkReady = false;
				blinkTimer = 0.3f;
				controller.Move(Input.GetAxisRaw("Horizontal") * HYPERSPEED * Time.fixedDeltaTime, false, false, true, isFacingRight, false);
			}
		}
	}

    void FixedUpdate()
    {
		// movement function
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, false, true, isFacingRight, false);
		controller.Move(verticalMove * Time.fixedDeltaTime, false, false, false, isFacingRight, false);

		// disable movement and other functions when stunned
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("EbotStun1") ||
			animator.GetCurrentAnimatorStateInfo(0).IsName("EbotStun2") ||
			animator.GetCurrentAnimatorStateInfo(0).IsName("EbotStun3"))
		{
			runSpeed = IDLE;

			isCloak = false;
			isSprint = false;
			isStrafe = false;

			animator.SetBool("isCloak", false);
			animator.SetBool("isSprint", false);
			animator.SetBool("isStrafe", false);
		}

		// return movement functions after stun
		// also works as a standard speed setter
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("EbotIdle"))
		{
			if (isStrafe == true)
			{
				runSpeed = SLOW;
			}
			else
			{
				runSpeed = NORMAL;
			}
		}

		// blink timer reset
		if (!blinkReady && blinkTimer > 0)
		{
			blinkTimer -= Time.deltaTime;
		}
		if (blinkTimer <= 0)
		{
			blinkReady = true;
			blinkTimer = 0;
		}
	}

    // damage function
    public void TakeDamage (int damage)
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