using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gale : Splash
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
        coolDown = 4f;
        //manaCost = 3;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    // public override void OnEndAttack()
    // {
    //     base.OnEndAttack();
    //     GameObject gusts = Instantiate(splashPrefab, startPos, transform.rotation);
    //     FaceTargetPos(gusts);
    //     gusts.GetComponent<Gust>().SetVariables(targetPos, Caster); 

    // }
    // public override void InitiateAttack(GameObject attacker)
    // {
    //     base.InitiateAttack(attacker);
    //     if (gameObject.tag == "Enemy")
    //     {
    //         targetPos = GameManager.PlayerPos;
    //     }
    // }
}
