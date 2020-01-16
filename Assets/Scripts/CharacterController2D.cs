using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [HideInInspector] public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    [HideInInspector] public Rigidbody2D m_Rigidbody2D;
    [HideInInspector] public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
    [HideInInspector] public bool m_IsStun = false;
	[HideInInspector] public bool m_IsInvincibility = false;

	// jump variable
	[SerializeField] private bool wallJump = false;
    [HideInInspector] public bool wallHit = false;
    [HideInInspector] public bool wallHitLeft = false;
    [HideInInspector] public bool wallHitRight = false;
    private int jumpCounter;
    private float speedLimit = 1.1f;
    private float jumpDelay;
    private bool jumpReady = true;

	// stun variable
	[HideInInspector] public float stunTimer;
	[HideInInspector] public float invincibilityTimer;

    [Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	
	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
    }


	public void Move(float move, bool crouch, bool jump, bool isXAxis, bool isFacingRight, bool firingStance)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if ((m_Grounded || m_AirControl) && (!m_IsStun))
		{
			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
            else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity;
			if (isXAxis)
			{
				targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			}
			else
			{
				targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, move * 10f);
			}
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // only flip when moving horizontally or not on firing stance
			if (isXAxis && !firingStance)
			{
				if (isFacingRight)
				{
					// If the input is moving the player right and the player is facing left...
					if (move > 0 && !m_FacingRight)
					{
						// ... flip the player.
						Flip();
					}
					// Otherwise if the input is moving the player left and the player is facing right...
					else if (move < 0 && m_FacingRight)
					{
						// ... flip the player.
						Flip();
					}
				}
				else
				{
					// If the input is moving the player right and the player is facing left...
					if (move < 0 && !m_FacingRight)
					{
						// ... flip the player.
						Flip();
					}
					// Otherwise if the input is moving the player left and the player is facing right...
					else if (move > 0 && m_FacingRight)
					{
						// ... flip the player.
						Flip();
					}
				}
			}
		}

		// If the player should jump...
		if ((m_Grounded && jump) && jumpReady)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            jumpCounter = 1;
            jumpReady = false;
            jumpDelay = 0.3f;
        }
        else if ((!m_Grounded && wallHit && jump) && (jumpCounter < 3) && jumpReady && wallJump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / speedLimit));
            jumpCounter++;
            wallHit = false;
        }

        // timer reset
        JumpReset();
		StunReset();
		InvincibilityReset();
    }

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0f);
	}
    
	/*// collision bug fix
    private void OnCollisionStay2D(Collision2D collision)
    {
        // stop enemy entities from moving when colliding
        if (collision.gameObject.tag != "TestChamber")
        {
            //m_Rigidbody2D.isKinematic = true;
            //m_Rigidbody2D.velocity = Vector3.zero;
            m_Rigidbody2D.drag = 500;
            m_Rigidbody2D.angularDrag = 500;
        }

		// disable drag when invincibility is active
		if (m_IsInvincibility && invincibilityTimer < 0.95f)
		{
			m_Rigidbody2D.drag = 0;
			m_Rigidbody2D.angularDrag = 0;
		}
	}
    private void OnCollisionExit2D(Collision2D collision)
    {
        // stop enemy entities from moving when colliding
        if (collision.gameObject.tag != "TestChamber")
        {
            //m_Rigidbody2D.isKinematic = false;
            m_Rigidbody2D.drag = 0;
            m_Rigidbody2D.angularDrag = 0;
        }
	}*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        // detect tilemaps for wall jump
        if (collision.gameObject.tag == "IndicatorLeft" && !wallHitRight && !m_Grounded)
        {
            wallHit = true;
            wallHitRight = true;
            wallHitLeft = false;
        }
        if (collision.gameObject.tag == "IndicatorRight" && !wallHitLeft && !m_Grounded)
        {
            wallHit = true;
            wallHitRight = false;
            wallHitLeft = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // detect tilemaps for wall jump
        if (collision.gameObject.tag == "IndicatorLeft")
        {
            wallHit = false;
        }
        if (collision.gameObject.tag == "IndicatorRight")
        {
            wallHit = false;
        }
    }

    // jump timer function
    private void JumpReset()
    {
        if (!jumpReady && jumpDelay > 0)
        {
            jumpDelay -= Time.deltaTime;
        }
        if (jumpDelay < 0)
        {
            jumpReady = true;
            jumpDelay = 0;
        }
    }

	// stun timer function
	private void StunReset()
	{
		if (m_IsStun && stunTimer > 0)
		{
			stunTimer -= Time.deltaTime;
		}
		if (stunTimer < 0)
		{
			m_IsStun = false;
		}
	}

	// invincibility timer function
	private void InvincibilityReset()
	{
		if (m_IsInvincibility && invincibilityTimer > 0)
		{
			invincibilityTimer -= Time.deltaTime;
		}
		if (invincibilityTimer < 0)
		{
			m_IsInvincibility = false;
		}
	}
}
