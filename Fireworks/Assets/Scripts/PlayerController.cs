using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveForce = 365f;					//Amount of force added to move the player left and right
	public float maxSpeed = 5f;						//The fastest the player can travel in the x axis

	public bool jump = false;						//Condition for whether the player should jump
	public float jumpForce = 1000f;					//Amount of force added when the player jumps

	private Transform groundCheck;					//A position marking where to check if the player is grounded.
	private bool grounded = false; 					//Whether or not the player is grounded

    private Animator anim;                          //Reference to the player's animator component

    public bool attack = false;                     //Condition for whether the player should attack. 

    public bool shootRecovery = false;              //Condition for whether the player is in ShootingRecovery.

    public bool attackButtonDown = false;           

	public bool isMoving = false;                   //Condition to check if the player is moving or not

	private Rigidbody2D rbody;                      //Variable that holds the Rigidbody of the object

    private SpriteRenderer sRenderer;               //Variable that holds the SpriteRenderer of the object

    public KeyCode left;                            //KeyCode for left button

    public KeyCode right;                           //KeyCode for right button

    public Rigidbody2D Arrow;                       //Variable that holds the Rigidbody for the Arrow

    public Rigidbody2D ArrowInstance;               //Variable that holds the Rigidbody for the ArrowInstance

    public Arrow ArrowInstanceComponent;

    public bool FireArrow = true;

    public float speed = 20f;                       //...What was this used for again?

    // Use this for initialization
	void Awake () {
		//setting up references
		groundCheck = transform.Find ("groundCheck");
        anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D> ();
        sRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

		// If the jump button is pressed and the player is grounded then the player should jump.
		if (Input.GetButtonDown ("Jump") && grounded) {
			jump = true;
		}

        if (Input.GetButton("Attack")== true)
        {
            attackButtonDown = true;
            attack = true;
        }

        if (Input.GetButton("Attack") == false)
        {
            attackButtonDown = false;
        }


        // If the attack button is pressed, set relevent variables. (One to play attack animation, one to other to run attack function) 
		/*if (Input.GetButton ("Attack") && attack == false) {
            attack = true;
            anim.SetBool("AttackOn", true);
            Shoot(Arrow);
            //Debug.Log("Update attack called");
		}

        // When attack button is let go, set relavent variables
		if (attack == true) {
            if (shootRecovery == false)
            {
                StartShootRecovery();
            }
            else if (shootRecovery == true && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackRecovery") == false && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack") == false)
            {
                EndShootRecovery();
            }
		}
         * */
        
        //if (shootRecovery == true && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackRecovery") == false && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack") == false)
        //{
          //  EndShootRecovery();
        //} 

        //Cache the horizontal input
        float h = Input.GetAxis("Horizontal");

        //The SpeedParam animator parameter is set to the absolute value of the horizontal input. 
        //anim.SetFloat("SpeedParam", Mathf.Abs(h));
		
    }

    // Mostly used for adding forces at the moment. 
	void FixedUpdate(){

        if (attackButtonDown)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackRecovery") == false && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack") == false)
            {
                Shoot(Arrow);
            }
        }
        else if (attack)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack") == true)
            {
                StartShootRecovery();
            }
        }
        else
        {
            //Cache the horizontal input
            float h = Input.GetAxis("Horizontal"); // GetAxis returns 1 and -1 

            if (h > 0)
            {
                sRenderer.flipX = false;
                
                
            }
            else if (h < 0)
            {
                sRenderer.flipX = true;
                
            }

            if (Input.GetKey("left") || Input.GetKey("right"))
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
           
            //The SpeedParam animator parameter is set to the absolute value of the horizontal input. 
            anim.SetFloat("SpeedParam", Mathf.Abs(h));
            anim.SetBool("isMoving", isMoving);
			//if(h != 0)
			//{
	            //If the player is changing direction (h has a different sign to velocity.x) or hasn't reached max speed yet...
	            if (h * rbody.velocity.x < maxSpeed)
	            {

	                //... add a force to the player
	                //rbody.AddForce(Vector2.right * h * moveForce);

                    // Change the player's velocity directly
					rbody.velocity = new Vector2(1f * h * moveForce, rbody.velocity.y);
	            }
	            //If the player's horizontal velocity is greater than the maxSpeed
	            if (Mathf.Abs(rbody.velocity.x) > maxSpeed)
	            {

	                //...set the player's velocity to the maxSpeed in the x axis
	                rbody.velocity = new Vector2(Mathf.Sign(rbody.velocity.x) * maxSpeed, rbody.velocity.y);
	            }
			//}
			//else
			//{
			//	rbody.velocity = new Vector2(0f, rbody.velocity.y);
			//}
            //If the player should jump...
            if (jump)
            {

                //Add a vertical force to the player
                rbody.AddForce(new Vector2(0f, jumpForce));

                //Make sure the player can't jump again until the jump conditions from Update are satisfied
                jump = false;
            }
        }
	}

    // Run the attacking sequence. 
    void Shoot(Rigidbody2D arrowShot)
    {
        //attack = true;
        anim.SetBool("AttackOn", true);
        //anim.SetBool("AttackRecoveryOn", true);

            if (FireArrow)
            {
                ArrowInstance = Instantiate(arrowShot, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                ArrowInstanceComponent = ArrowInstance.GetComponent<Arrow>();
                //ArrowInstanceComponent.StartDestroyCountdown();
                FireArrow = false;
                //ArrowInstance.velocity = new Vector2(speed, 0);
            }
            

            //Set both velocity and angularVelocity to 0f
            rbody.velocity = Vector2.zero;
            rbody.angularVelocity = 0f;

            //The effect of gravity on the character must also be set to zero. However this value must be changed back to 1 when the character is done attacking. 	
            rbody.gravityScale = 0f;

            Debug.Log("Shoot called");
    }

    void StartShootRecovery()
    {
        
        shootRecovery = true;
        //anim.SetBool("AttackOn", false);
        //attack = false;
        Debug.Log("StartShootRecovery called");
        if (sRenderer.flipX == false)
        {
            ArrowInstance.velocity = new Vector2(speed, 0);
        }
        else
        {
            ArrowInstance.velocity = new Vector2(-1 * speed, 0);
        }

        ArrowInstanceComponent.StartDestroyCountdown();

        anim.Play("PlayerAttackRecovery");

        //ArrowInstanceComponent.StartDestroyCountdown();

        //EndShootRecovery();
        
    }

    void EndShootRecovery()
    {
       /* //Reinstate the gravity if the player is no longer attacking. Sets attack animation parameters to false.
        //rbody.gravityScale = 1f;
        anim.SetBool("AttackRecoveryOn", false);
        anim.SetBool("AttackOn", false);
        attack = false;
        shootRecovery = false;
        Debug.Log("EndShootRecovery called");*/
    }

	public void ReinstateGravity()
	{
		rbody.gravityScale = 1f;
        anim.SetBool("AttackRecoveryOn", false);
        anim.SetBool("AttackOn", false);
        attack = false;
        shootRecovery = false;
        Debug.Log("EndShootRecovery called");
        FireArrow = true;
        //ArrowInstanceComponent.StartDestroyCountdown();
	}


}
