using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

// private NavMeshAgent navMeshAgent;

	// public enum AnimationState
	// {
	// 	castSpell,
	// 	idle,
	// 	move,
    //     stunned
	// 	// hurt,
	// 	// die
	// }
	
    // public static float maxSpeed = 6f;
    Rigidbody rb;
    private bool moving;
    private bool stunned;
    public bool Stunned
	{
        get 
        {
            // if (stunned && stats.Health >= 5f&&!GameManager.InTutorial)
            // {
            //     // anime.SetTrigger("recovered");
            //     // stunned = false;
            // }
            if (GameManager.Immortal)
            {
                return false;
            }
            return stunned;
        }
    }
    bool attacking;
    public bool PlayerIsAttacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
        }
    }

    // public bool Casting
    // {
    //     set { casting = value;
    //         if (casting == true)
    //         {
    //             anime.SetTrigger("Attack");
    //         }
    //     }
    // }
    Vector3 targetDirection;
    float rotationSpeed = 30f;

    bool isPaused = false;
    PlayerStats stats;
    Animator anime;
    float lastHurt;
    float hurtCooldown= 1f;
    MeleeWeapon punch;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();
        stats = GameManager.player.GetComponent<PlayerStats>();
        punch = GetComponentInChildren<Punch>();
        // navMeshAgent = GetComponent<NavMeshAgent>();
        // navMeshAgent.speed = moveSpeed;
    }

    void Update()
    {
        if (!PlayerIsAttacking&&!stunned)
        {
            if (Input.GetKeyDown(Controls.GetControl("off_hand")))
            {
                attacking = true;
                anime.SetBool("Unarmed", true);
                anime.SetTrigger("Attack");
                punch.InitiateAttack(gameObject);
            }
            Movement();
        }
        else
        {
            moving = false;
        }
        UpdateAnimator();

        // float z = Input.GetAxisRaw("Horizontal");
		// float x = -(Input.GetAxisRaw("Vertical"));
		//inputVec = new Vector3(x, 0, z);

		//Apply inputs to animator
		
    }
    void TakeDamage(float amount)
	{
        //AnimatorClipInfo[] currentAnimation = anime.GetCurrentAnimatorClipInfo(0);
        attacking = false;
        GameManager.Cantis.GetComponent<PlayerSpells>().SpellInterupted();
        if (stats != null)
        {
            if (stats.Health <= 0)
            {
                anime.SetTrigger("die");
                anime.SetBool("dead",true);
                stunned = true;
                if (GameManager.Killable&&!GameManager.Immortal)
                {
                    PlayerDied();
                }
                // stats.mindState = StateOfMindEnum.dead;
                //GetComponent<Collider>().enabled = false;
            }
            else if (Time.time >= hurtCooldown + lastHurt)
            {
                lastHurt = Time.time;
                if (anime != null)
                {
                    anime.SetTrigger("hurt");
                }
            }
        }
    }

	void TakeDamage(Dictionary<Element, float> elems)
	{
        attacking = false;
        GameManager.Cantis.GetComponent<PlayerSpells>().SpellInterupted();
        //AnimatorClipInfo[] currentAnimation = anime.GetCurrentAnimatorClipInfo(0);
        if (stats != null)
        {
            if (stats.Health <= 0)
            {
                anime.SetTrigger("die");
                anime.SetBool("dead",true);
                stunned = true;
                if (GameManager.Killable&&!GameManager.Immortal)
                {
                    PlayerDied();
                }
                // stats.mindState = StateOfMindEnum.dead;
                //GetComponent<Collider>().enabled = false;
            }
            else if (Time.time >= hurtCooldown + lastHurt)
            {
                lastHurt = Time.time;
                if (anime != null)
                {
                    anime.SetTrigger("hurt");
                }
            }
        }
    }
    void AttackingAnimationDone()
    {
        anime.SetBool("Unarmed", false);
        attacking = false;
    }
    void UpdateAnimator()
    {
        // if (casting)
        // { 
        //     anime.SetTrigger("Attack")
        // }
        anime.SetBool("Moving", moving);
    }
    void Movement()
    {
        float vertical = 0;
        float horizontal = 0;
        GameManager.Sprinting = Input.GetKey(Controls.GetControl("sprint"));//TODO: Get rid of outside testing?
        if (Input.GetKey(Controls.GetControl("character_up")) && !Input.GetKey(Controls.GetControl("character_down")))
        {
            vertical++;
            horizontal++;
        }
        if (Input.GetKey(Controls.GetControl("character_down")) && !Input.GetKey(Controls.GetControl("character_up")))
        {
            vertical--;
            horizontal--;
        }

        if (Input.GetKey(Controls.GetControl("character_right")) && !Input.GetKey(Controls.GetControl("character_left")))
        {
            vertical--;
            horizontal++;
        }
        if (Input.GetKey(Controls.GetControl("character_left")) && !Input.GetKey(Controls.GetControl("character_right")))
        {
            vertical++;
            horizontal--;
        }


        if (horizontal != 0 || vertical != 0)
        {
            moving = true;
        }
        else 
        {
            moving = false;
        }

        Vector3 movingVelocity = new Vector3(horizontal, 0f, vertical);
        movingVelocity.Normalize();
        movingVelocity *= Time.deltaTime * GameManager.PlayerSpeed;

        rb.velocity = movingVelocity;
        float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		// Target direction relative to the camera
		targetDirection = rb.velocity;
        FaceDirection();
        //transform.Translate(0, 0, z);
    }

    void FaceDirection()
    { 
        if(rb.velocity != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
		}
    }
    // Use this for initialization
    
    void Hit()
	{
        punch.OnEndAttack();
    }

	void FootR()
	{
	}

	void FootL()
	{
	}

    void EndAttack()
    {
        GameManager.Cantis.GetComponent<PlayerSpells>().EndAttack();
    }
    void PlayerDied()
    {
        print("dead");
        gameObject.GetComponent<PlayerUIScript>().DeadMenu();
    }
    public void Recovered()
    {
        stunned = false;
        anime.SetTrigger("recovered");
        anime.SetBool("dead", false);
    }
}
