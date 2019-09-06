using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a more comprehensive enemies script for attacks which is easier to inherit from. It stills rely on enemyAI1 for states and movement
/// </summary>
public class EnemyAttackScript : MonoBehaviour {

    // Use this for initialization
    protected Attack enemiesAttack;
    protected Animator anime;
    public Attack EnemiesAttack
    {
        get { return enemiesAttack; }
    }
    bool attacking;

    public virtual void Start () {
        enemiesAttack = GetComponent<Attack>();
        if (enemiesAttack == null)
        { 
			enemiesAttack = GetComponentInChildren<Attack>();
		}
        anime = GetComponent<Animator>();
    }
    public virtual void SetUpAttack()
    { 
        anime.SetTrigger("attack");
        //GetComponent<EnemyAttackScript>().Attack();
        attacking = true;
        anime.SetBool("moving", false);
    }

    // Update is called once per frame
    void Update () {
		
	}

	public virtual void CueAttack()
	{
        enemiesAttack.InitiateAttack(gameObject);
    }
    public virtual void EndAttack()
    {
        enemiesAttack.OnEndAttack();
    }
    public virtual Vector3 FindTarget()
    {
        return GameManager.PlayerPos;
    }
}
