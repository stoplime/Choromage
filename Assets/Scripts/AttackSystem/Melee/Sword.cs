using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temp script for sword. Convert to struct ???
/// </summary>
public class Sword : MeleeWeapon {
	// Quaternion resetRotation;

    // Use this for initialization
    public override void Start () {
		base.Start();
        twoHanded = false;
        swinging = true;
		Damage = 10f;
    }
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

        //TODO: change condidtion~~~
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     // print("stuff");
        //     InitiateAttack();
        // }
	}

    // public override void InitiateAttack()
    // {
    //     base.InitiateAttack();
    // }

    // public override void AttackUpdate()
    // {
    //     base.AttackUpdate();
    //     //transform.Rotate(Vector3.left * 10);
    // }

	// public override void OnEndAttack()
	// {
    //     base.OnEndAttack();
	// 	//transform.rotation = resetRotation;
	// }

}
