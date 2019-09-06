using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MeleeWeapon {

    static Collider fist;
    // Use this for initialization
    public override void Start () {
        base.Start();
        Damage = 3f;
        fist = GetComponent<Collider>();

        // print(fist);
        // print(GetComponentsInChildren<Collider>().Length);
        // weapon.SetActive(false);

        // print(weaponCollider.enabled);
        // fist = GetComponent<Collider>();
        fist.enabled = false;
    }
    public override void InitiateAttack(GameObject attacker)
	{
        base.InitiateAttack(attacker);
        print(fist);
        // fist.SetActive(true);
        fist.enabled = true;
        // weaponCollider.enabled = true;
    }
    public override void OnEndAttack()
    {
        //resets position as a placeholder for animation
        // transform.localRotation = resetRotation;
        fist.enabled = false;
        // fist.SetActive(false);
        // weaponCollider.enabled = false;
    }
    
    // void StartAttack()
    // {
    //     // fist.enabled = true;
    // }
    // public void Hit()
    // { 

    // }
    // Update is called once per frame
    // void Update () {

    // }
}
