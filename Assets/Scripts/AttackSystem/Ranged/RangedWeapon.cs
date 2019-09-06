using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to encompass all ranged weapons/attacks including bows, crossbows, and throwing rocks. Each child should call the projectile class (or its children). Might be able to use structs instead of classes for each weapon/attak. 
/// </summary>
public class RangedWeapon : Attack
{
    protected GameObject projectilePrefab;
    protected float projectileSpeed = .25f;
    protected Vector3 targetPos;
    protected GameObject projectile;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void InitiateAttack(GameObject attacker)
    {
        //play animation animation
        //copy paste for each
        //projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        //projectile.GetComponent<Projectiles>().SetSpeedAndDamage(projectileSpeed, Damage, GetComponentInParent<Collider>());
        base.InitiateAttack(attacker);
    }

    public void FaceTargetPos(GameObject splashy)
    {
        splashy.transform.rotation = Quaternion.identity;
        targetPos = GameManager.PlayerPos;
        Vector3 tempPos = targetPos;
        tempPos.y = splashy.transform.position.y;
        splashy.transform.rotation = Quaternion.LookRotation(tempPos - splashy.transform.position);
    }

    public override void AttackUpdate()
    {
        base.AttackUpdate();
    }

    public override void OnEndAttack()
    {
        //after animation projectile will leave parent gameobject
        
        //instantiate projectiles 
        // GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        // projectile.GetComponent<Projectiles>().SetSpeedAndDamage(projectileSpeed, Damage, GetComponentInParent<Collider>());
        
        base.OnEndAttack();
        if (projectile != null)
        {
            FaceTargetPos(projectile);
            projectile.transform.parent = null;
        }
    }
}