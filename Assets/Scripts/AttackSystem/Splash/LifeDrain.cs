using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDrain : Splash
{

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        coolDown = 5f;
        //manaCost = 20;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    // public override void OnEndAttack()
    // {
    //     base.OnEndAttack();
    //     GameObject healingBub = Instantiate(splashPrefab, startPos, transform.rotation);
    //     healingBub.GetComponent<HealingBubble>().SetVariables(targetPos, Caster);
    // }

    public override void InitiateAttack(GameObject attacker)
    {
        base.InitiateAttack(attacker);
        targetPos = transform.position;
    }
    
}
