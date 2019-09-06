using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : RangedWeapon {

    GameObject target;
    public override void Start()
    {
        projectilePrefab = Resources.Load("projectiles/Rocky") as GameObject;
        AttackDuration = .5f;
        projectileSpeed = 10;
        target = GameManager.player;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();       
    }

    public override void InitiateAttack(GameObject attacker)
    {
        projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<Rock>().SetSpeedAndDamage(projectileSpeed, Damage, gameObject.GetComponentsInParent<Collider>());
        base.InitiateAttack(attacker);
    }

    public override void AttackUpdate()
    {
        base.AttackUpdate();
    }

    public override void OnEndAttack()
    {
        base.OnEndAttack();
    }
}