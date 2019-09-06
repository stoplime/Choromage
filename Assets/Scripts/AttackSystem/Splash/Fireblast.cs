using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireblast : Splash
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
    //     if (gameObject.tag == "Enemy")
    //     {
    //         //print(name);
    //         targetPos = GameManager.PlayerPos;
    //     }

    // }

    //  public override void OnEndAttack()
    // {
    //     base.OnEndAttack();
    //     GameObject fireAttack = Instantiate(splashPrefab, startPos, transform.rotation);
    //     fireAttack.GetComponent<Fireball>().SetVariables(targetPos, Caster);
    // }
}

