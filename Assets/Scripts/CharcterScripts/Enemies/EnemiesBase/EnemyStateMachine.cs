using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum StateOfMindEnum
{
	passive,
	guarded,
	aggressive,
	defensive,
	dead
	
}
public enum StateOfBeingEnum
{
	attack,
	//hurt,
	idle,
	move,
	pushed
	//die
}
public struct AIState
{
	public StateOfMindEnum mindState;
	public StateOfBeingEnum beingState;

	public AIState (StateOfMindEnum mindSet, StateOfBeingEnum beingSet)
	{
		mindState = mindSet;
		beingState =beingSet;
	}
}

/// <summary>
/// Parent class that tracks enemy AI through states
/// </summary>
public class EnemyStateMachine : MonoBehaviour {

	public Movement movement;

	protected AIState state = new AIState(StateOfMindEnum.passive, StateOfBeingEnum.idle);
	protected Animator anime;
	bool dying;
    bool beingPushed;

    /// <summary>
    /// How fost enemy moves based on it's current state (StateOfMind). TODO: actual set it equal to the state's stat
    /// </summary>
    protected float currentMoveSpeed =3f;

	/// <summary>
	/// How far an enemy can see player based on it's state  (StateOfMind). TODO: actual set it equal to the state's stat
	/// </summary>
 	 protected float currentSightDistance =15f;

	/// <summary>
	/// Amount of distance that enemy wants between them and player. currently its the attack distance. TODO: Incorporatate distace it want when on the defensive/running away
	/// </summary>
     protected float currentBufferDistance = 2.5f;

	protected EnemyAttackScript attackScript;

	protected bool seesPlayer;
	protected bool seenPlayer;

	//float timeSincePlayerSeen;

	/// <summary>
	/// so hurt doesn't start again in mid hurt anuimation
	/// </summary>
	protected float hurtCooldown = 1f;
	protected float lastHurt=-1f;

    protected GameObject eyes;

    protected bool attacking;

    protected EnemyStats enemysStats;

	protected List<ItemSaveName> loot;

	protected float passiveMoveIdleTimer;
    protected float passiveRandomTime =10;

    private bool insideCollider;

    public bool InsideCollider
    {
        get { return insideCollider; }
    }

    protected float attackCooldown=0;

    public virtual void Start () {
		attackScript = GetComponent<EnemyAttackScript>();
		anime = GetComponent<Animator>();
        FindEyes();
        CreateMovementScript();
        enemysStats = GetComponent<EnemyStats>();
		loot = new List<ItemSaveName>();
		loot.Add(new ItemSaveName("Rock", 1));
    }

    //TODO: change to attribute?
    public virtual void FindEyes()
    { 
		eyes = gameObject.transform.Find("Eyes").gameObject;
	}

    void CreateMovementScript()
    { 
		movement = gameObject.AddComponent<Movement>();
        movement.CreateMovement(1f, eyes.gameObject);
	}
	public virtual void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Wind")
        {
            beingPushed = true;
        }
    }
	public virtual void OnTriggerExit(Collider other)
    {

        if (other.tag == "Wind")
        {
            beingPushed = false;
        }
    }


    // Update is called once per frame
    void Update () {
		CheckChunkDespawn();
		if (state.mindState!= StateOfMindEnum.dead)
		{
			//UpdateHealth();
			
			Sight();
			UpdateMindState();
			UpdateStateStats();
            UpdateBeing();
			WhereToFace();
			CheckBeing();
            //CheckAttack();
        }
		else
		{
			anime.SetInteger("mindSet", (int)state.mindState);
		}
        if (transform.position.y < -5f)
        {
            DoneDying();
        }
    }

	protected virtual void CheckChunkDespawn()
	{
		if (SceneManager.GetActiveScene().name == "EarlyEnvironment")
			return;
		Vector2Int chunkCoord = new Vector2Int((int)Mathf.Floor(transform.position.x / ChunkManager.Chunk2GlobalRate),
                                               (int)Mathf.Floor(transform.position.z / ChunkManager.Chunk2GlobalRate));
		if (!ChunkManager.ChunkExists(chunkCoord))
		{
			Destroy(gameObject);
		}
	}

	void Sight()
	{
        //print(currentSightDistance);
        movement.SpotTarget(GameManager.player, currentSightDistance);
        //print(movement.TargetInRange);
        //Help.print((bool)(movement.TargetInRange), (bool)(movement.TargetSightIndex == 0));
        //print(movement.TargetSightIndex);
        //if (movement.TargetInRange && (movement.TargetSightIndex == 0))
        // print(GameManager.Invisible);
        // Help.LiveDebugText("Invisible", GameManager.Invisible);
        if (movement.TargetInRange&&!GameManager.Invisible)
		{
            seenPlayer = true;
            seesPlayer = true;
        }
		else
		{
			seesPlayer = false;
		}
    }

	float DistanceFromPlayer()
	{
		return Vector3.Distance(transform.position, GameManager.PlayerPos);
	}

	void UpdateMindState()
	{

        // if (DistanceFromPlayer() <= currentSightDistance+10f)
        // {
        //     Help.print(seesPlayer, GameManager.Invisible);
        // }
        if (GameManager.Invisible&&seenPlayer)
        { 
			state.mindState =  StateOfMindEnum.guarded;
		}
        else if (seesPlayer)
		{
			state.mindState =  StateOfMindEnum.aggressive;
            //print(state.mindState.ToString());
			// UpdateStateStats();
        }
		else if (seenPlayer)
		{
			state.mindState =  StateOfMindEnum.guarded;
		}
		// else
		// {
		// 	state.mindState =  StateOfMindEnum.passive;
		// }
		anime.SetInteger("mindSet", (int)state.mindState);
        
    }

	/// <summary>
	/// Changes beingState based on mindState. It also takes into account distance to player.
	/// </summary>
	public virtual void UpdateBeing()
	{
        //print(state.mindState);
        switch (state.mindState)
			{
				case StateOfMindEnum.passive:
					break;
				case StateOfMindEnum.aggressive:
					PlayAggressiveSound();
					if (beingPushed)
					{ 
						state.beingState = StateOfBeingEnum.pushed;
					}
					else if (DistanceFromPlayer() > currentBufferDistance)
					{
						state.beingState = StateOfBeingEnum.move;
						// print("agressive");
					}
					else if (state.beingState == StateOfBeingEnum.idle)
					{
						
						state.beingState = StateOfBeingEnum.attack;
					}
					else
					{
						state.beingState = StateOfBeingEnum.idle;
					}
					break;

				case StateOfMindEnum.defensive:
					if (DistanceFromPlayer() < currentBufferDistance)
					{
						state.beingState = StateOfBeingEnum.move;
					}
					else
					{
						state.beingState = StateOfBeingEnum.idle;
					}
					break;

				case StateOfMindEnum.guarded:
					if (beingPushed)
					{ 
						state.beingState = StateOfBeingEnum.pushed;
					}
					else if (state.beingState == StateOfBeingEnum.move)
					{
						state.beingState = StateOfBeingEnum.idle;
					}
					break;
				default:
					break;
			}
	}
	
	//TODO: STEFFEN: You may need to look at this when creating the movement AI.~~~
	/// <summary>
	/// Decides where the enemy should face mostly based off of their mindeState.
	/// </summary>
	public virtual void WhereToFace()
	{
		switch (state.mindState)
			{
				case StateOfMindEnum.passive:
					break;
				case StateOfMindEnum.aggressive:
                	FacePlayer();
					// if (seesPlayer)
                	// {
                    // 	FacePlayer();
                	// }
                break;

				case StateOfMindEnum.defensive:
					// faces away to flee. faces towards to stand ground and fight
					if (DistanceFromPlayer() < currentBufferDistance)
					{
						FaceAwayFromPlayer();
					}
					else
					{
						FacePlayer();
					}
					break;

				case StateOfMindEnum.guarded:
					if (state.beingState == StateOfBeingEnum.move)
					{
						state.beingState = StateOfBeingEnum.idle;
					}
					break;
				default:
					break;
			}
	}

    void AttackDone()
    {
        attacking = false;
    }

    // void PassiveIdleMoveTimer()
    // {
    //     passiveMoveIdleTimer += Time.deltaTime;
    //     if (passiveMoveIdleTimer >= passiveRandomTime)
    //     {
    //         passiveMoveIdleTimer = 0;
	// 		//passiveRandomTime = 
    //     }
    // }

    //STEFFEN: 	This method contains where movement is being called~~~
    /// <summary>
    /// Figures out what the enemy is doing and does so accordingly. Sets bools and triggers for animation based off of state of being. Also tells enemy to move if beingState == move
    /// </summary>
    public virtual void CheckBeing()
	{
		switch (state.beingState)
		{
			case StateOfBeingEnum.attack:
                attacking = true;
                attackScript.SetUpAttack();
                PlayAttackSound();
                // anime.SetTrigger("attack");
                // //GetComponent<EnemyAttackScript>().Attack();
                // attacking = true;
                anime.SetBool("moving", false);
                // //print("attack");
                // //attackScript.Attack();
                break;
			case StateOfBeingEnum.move:
                // if (!attacking)
                // {
					
					anime.SetBool("moving", true);
                    //Debug.Log("moving");
                    //STEFFEN this is where movement is being called~~~
                    Move();
                // }
                break;
			default:
				anime.SetBool("moving", false);
				break;
		}
	}

	/// <summary>
	/// Updates floats based on state of mind
	/// </summary>
	void UpdateStateStats()
	{
        //print("USS " + state.mindState.ToString());
        currentMoveSpeed = enemysStats.GetSpeed(state.mindState.ToString());
		currentSightDistance = enemysStats.GetSight(state.mindState.ToString());
        
        currentBufferDistance = enemysStats.GetBuffer(state.mindState.ToString());
		// if (gameObject.name == "wolf" )
        // {
        //    Help.print(currentBufferDistance, state.mindState.ToString());
        //}
        //if ()
        //currentMoveSpeed = enemysStats. ;
        //currentSightDistance = currentMindset.SightDistance;
		// if (gameObject.name == "TutorialWolf")
		// {
        //     Help.LiveDebugText(currentMoveSpeed,currentSightDistance,currentBufferDistance);
        // }
    }
	/// <summary>
	/// Tells the enemy how to move.virtual Curently its only forward.
	/// </summary>
	public virtual void Move()
	{
        if (!attacking&&!beingPushed)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * currentMoveSpeed*GameManager.UniversalSpeedMultiplier);
        }
    }

	protected void TakeDamage(float amount)
	{
        //AnimatorClipInfo[] currentAnimation = anime.GetCurrentAnimatorClipInfo(0);
        if (enemysStats != null)
        {
            if (enemysStats.Health <= 0&&!dying)
            {
                dying = true;
                anime.SetTrigger("die");
                state.mindState = StateOfMindEnum.dead;
                PlayDeathSound();
                //GetComponent<Collider>().enabled = false;
            }
            else if (Time.time >= hurtCooldown + lastHurt&&enemysStats.Health > 0)
            {
                lastHurt = Time.time;
                if (anime != null)
                {
                    anime.SetTrigger("hurt");
                    PlayHurtSound();
                }
            }
        }
    }

	protected void TakeDamage(Dictionary<Element, float> elems)
	{
        //AnimatorClipInfo[] currentAnimation = anime.GetCurrentAnimatorClipInfo(0);
        if (enemysStats != null)
        {
            if (enemysStats.Health <= 0&&!dying)
            {
                dying = true;
                anime.SetTrigger("die");
                state.mindState = StateOfMindEnum.dead;
                PlayDeathSound();
                //GetComponent<Collider>().enabled = false;
            }
            else if (Time.time >= hurtCooldown + lastHurt&&enemysStats.Health > 0)
            {
                lastHurt = Time.time;
                if (anime != null)
                {
                    anime.SetTrigger("hurt");
                    PlayHurtSound();
                }
            }
        }
    }
    void DestroyCollider()
    {
        //print("gob "+GetComponent<Collider>());
        GetComponent<Collider>().enabled = false;
	}
    protected virtual void DoneDying()
	{
		Vector3 lootBagPos = new Vector3 (transform.position.x, 0, transform.position.z);
		GameObject lootObj = Instantiate(Resources.Load("LootBag"), lootBagPos, Quaternion.identity) as GameObject;
		lootObj.name = "LootBag";
		Inventory lootInventory = lootObj.GetComponent<Inventory>();
		lootInventory.InitialItems = loot;
		lootInventory.slotCount = loot.Count;
		Destroy(gameObject);
	}

	// /// <summary>
	// /// Calls DoneDying() when trigger collider touches ground. It assumes that the trigger will only touch the ground if enemy has finished playing death animation
	// /// </summary>
	// /// <param name="other"></param>
	// void OnTriggerEnter(Collider other)
	// {
	// 	if (other.tag == "Ground")
	// 	{
	// 		//DoneDying();
	// 	}
	// }

	void OnCollisionEnter(Collision other)
	{
        //print("colide");
		//9 =environtment, 4= water
        if (other.gameObject.layer == 9||other.gameObject.layer == 4||other.gameObject.layer == 12)
		{
            //print("obs");
            insideCollider = true;
        }
	}

	/// <summary>
    /// This rotates the enemy to face the player they are attacking. While Quaternion.LookAt could be used with the player's and enemies positions, the y values of the tranforms could be different resulting in the enemy rotating on the x/z axis rather than just the y axis. To fix this a temp vec3 is created for the player's position and reassigns the y to equal the enemies
    /// </summary>
    void FacePlayer()
    {
        Vector3 tempPlayerPos = GameManager.PlayerPos;
        tempPlayerPos.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation(tempPlayerPos- transform.position);
    }

	/// <summary>
    /// Rotates away from player.LookAt could be used with the player's and enemies positions, the y values of the tranforms could be different resulting in the enemy rotating on the x/z axis rather than just the y axis. To fix this a temp vec3 is created for the player's position and reassigns the y to equal the enemies
	/// TODO: Test
    /// </summary>
    void FaceAwayFromPlayer()
    {
        Vector3 tempPlayerPos = GameManager.PlayerPos;
        tempPlayerPos.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation( transform.position - tempPlayerPos);
    }
    protected virtual void PlayAttackSound()
    { 
		
	}
	protected virtual void PlayAggressiveSound()
    { 
		
	}
	protected virtual void PlayHurtSound()
    { 
		
	}
	protected virtual void PlayDeathSound()
    { 
		
	}
}
