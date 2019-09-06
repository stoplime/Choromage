using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpittle : Splash
{
    public override bool NeedsTarget
    {
        get
        {
            return true;
        }
    }


    // Use this for initialization
    public override void Start()
    {
        base.Start();
        splashPrefab = Resources.Load("splashes/Fireball") as GameObject;
        caster = gameObject;
        //coolDown = 1f;
    }
    public void Awake()
    { 
        splashPrefab = Resources.Load("splashes/Fireball") as GameObject;
        caster = gameObject;
        //coolDown = 1f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    // public override void InitiateAttack(GameObject attacker)
    // {
    //     base.InitiateAttack(attacker);
    //     if (attacker.tag == "Enemy")
    //     {
    //         //print(name);
    //         targetPos = GameManager.PlayerPos;
    //     }

    // }

    public override void OnEndAttack()
    {
        //base.OnEndAttack();
        //TODO: narrw down where startPos goes wrong
        //GameObject fireAttack = Instantiate(splashPrefab, Caster.transform.position, transform.rotation);
        GameObject fireAttack = Instantiate(splashPrefab, startPos, transform.rotation);
        //TODO: Refactor
        fireAttack.GetComponent<Fireball>().SetVariables(targetPos, Caster, lingeringDuration, maxDiameter, elements, damage);
        // print(targetPos);
        // fireAttack.GetComponent<Fireball>().SetVariables(targetPos, Caster);
		//fireAttack.transform.localScale = new Vector3(.5f,.5f,.5f);
    }

    // public override void InitiateAttack(GameObject attacker, Vector3 pos)
    // {
    //     targetPos = pos;
    //     InitiateAttack(attacker);
    // }
    // public override void InitiateAttack(GameObject attacker)
    // {
    //     caster = attacker;
    //     if (caster.tag == "Enemy")
    //     {
    //         targetPos = GameManager.PlayerPos;
    //     }
    //     base.InitiateAttack(attacker);
    //     startPos = transform.position;
    //     //cooledDown = false;
    // }
    
}

