using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Splash {

	// Use this for initialization
	// public override void Start()
    // {
    //     base.Start();
    //     coolDown = 3f;
    // }
	
	// Update is called once per frame
	// public override void Update()
    // {
    //     base.Update();
    // }

    // public override void OnEndAttack()
    // {
    //     base.OnEndAttack();
    //     GameObject freezeAttack = Instantiate(splashPrefab, startPos, transform.rotation);
    //     freezeAttack.GetComponent<IceBlast>().SetVariables(targetPos, Caster);
        
    // }

	public override void InitiateAttack(GameObject attacker)
    {
		base.InitiateAttack(attacker);
        targetPos = transform.position;
    }
}
