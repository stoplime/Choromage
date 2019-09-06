using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : RangedWeapon {

    public override void Start()
    {
        projectilePrefab = Resources.Load("projectiles/Arrow") as GameObject;
        AttackDuration = .1f;
        projectileSpeed = 10;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void InitiateAttack(GameObject attacker)
    {
        //play bow animation
        base.InitiateAttack(attacker);
    }

    public override void AttackUpdate()
    {
        base.AttackUpdate();
    }

    public override void OnEndAttack()
    {
        GameObject arrow = Instantiate(projectilePrefab, transform.position, transform.rotation);
        arrow.GetComponent<Arrow>().SetSpeedAndDamage(projectileSpeed, Damage,GetComponentInParent<Collider>());
    }
}
