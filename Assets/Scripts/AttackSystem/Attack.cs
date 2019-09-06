using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	protected float Damage;


    /// <summary>
    /// Will represent length of attack animation
    /// </summary>
	protected float AttackDuration;

	protected bool isAttacking;

	private float timer;

    protected GameObject attackerRoot;

    protected List<Collider> attackerAdndFriends;
	public bool attackCooledDown;

    public virtual bool NeedsTarget
    {
        get { return false; }
    }

	public string AttackTime
	{
		get { 
			if (AttackDuration-timer<1)
			{
				return System.String.Format("{0:0.##}", (AttackDuration-timer));
			}
			else
			{
				return System.String.Format("{0:#.##}", (AttackDuration-timer));
			}
		}
	}
	// private bool 
	// protected float 

	// Use this for initialization
	public virtual void Start () {

	}
	
	// Update is called once per frame
	public virtual void Update () {
        if (isAttacking)
		{
			timer += Time.deltaTime;
			if (timer >= AttackDuration)
			{
				timer = 0;
				attackCooledDown = false;
				//OnEndAttack();
			}
			AttackUpdate();
		}
	}

	/// <summary>
	/// Attack update runs only during an attack
	/// </summary>
	public virtual void AttackUpdate()
	{

    }

	/// <summary>
	/// Runs this script at the end of an attack
	/// Useful for resetting attacks
	/// </summary>
	public virtual void OnEndAttack()
	{
		isAttacking = false;
	}

	/// <summary>
	/// Starts the Attack
	/// </summary>
	public virtual void InitiateAttack(GameObject attacker)
	{
        attackerRoot = attacker;
        if (!GameManager.IsPaused)
		{
			isAttacking = true;
		}
	}
    public virtual void InitiateAttack(GameObject attacker,Vector3 pos)
    {

    }
}
