using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDScript : MonoBehaviour
{
	public CharacterController2D controller;
	public Animator animator;
    public bool isFacingRight;

    // movement speed variable
    public float runSpeed = 20f;
	float horizontalMove = 0f;

	// constant speed variable
	const float IDLE = 0f;
	const float SLOW = 10f;
	const float NORMAL = 20f;
	const float FAST = 35f;
	const float HYPERSPEED = 1000f;

	// boolean variable
	bool isSprint = false;
    bool isJump = false;
    bool isFall = false;
    bool isAirborne = false;

    // damage variable
    public int health = 100;

    void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
	{
		// calculated movement speed variable
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		// speed tracker variable
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		// sprint function
		if (Input.GetKey(KeyCode.LeftShift))
		{
			animator.SetBool("isSprint", true);
			isSprint = true;
			runSpeed = FAST;
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			animator.SetBool("isSprint", false);
			isSprint = false;
			runSpeed = NORMAL;
		}

        // jump function
        if (Input.GetButtonDown("Jump") && controller.m_Grounded && !isFall)
        {
            animator.SetBool("isJump", true);
            isJump = true;
            isAirborne = true;
        }
        else if (Input.GetButton("Jump") && !controller.m_Grounded && !isFall)
        {
            animator.SetBool("isJump", true);
            isJump = true;
            isAirborne = true;
        }

        // fall function when entity is airborne without jumping
        if (!controller.m_Grounded && !isAirborne)
        {
            animator.SetBool("isFall", true);
            isFall = true;
        }
        else if (controller.m_Grounded)
        {
            isFall = false;
            isAirborne = false;
        }
    }

    // landing function
    public void OnLanding()
    {
        animator.SetBool("isJump", false);
        animator.SetBool("isFall", false);
        controller.wallHit = false;
        controller.wallHitLeft = false;
        controller.wallHitRight = false;
    }

	void FixedUpdate()
	{
		// movement function
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, isJump, true, isFacingRight, false);
        isJump = false;

		if (!controller.m_IsStun)
		{
			animator.SetBool("isHit", false);
		}
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

    // stun knockback function
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && ((!controller.m_IsInvincibility) || (controller.invincibilityTimer > 0.95f)))
        {
            if (controller.m_FacingRight)
                controller.m_Rigidbody2D.AddForce(new Vector2(2000f, 4000));
            else if (!controller.m_FacingRight)
                controller.m_Rigidbody2D.AddForce(new Vector2(-2000f, 4000));

            controller.m_IsStun = true;
            controller.m_IsInvincibility = true;
            animator.SetBool("isHit", true);
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", true);
            isJump = false;
            isFall = true;

            controller.stunTimer = 0.5f;
            controller.invincibilityTimer = 1.0f;
        }
    }
}