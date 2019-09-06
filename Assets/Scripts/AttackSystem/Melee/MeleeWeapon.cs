using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to encompass all melee weapons/attacks including swords, axes, and spears. Might be able to use structs instead of classes for each
/// </summary>
public class MeleeWeapon : Attack
{
    //TODO: get rid animation standins once animation is implmented~~~
    // Quaternion resetRotation;

    /// <summary>
    /// true means its a swinging weapon (play swing weapon animation), false means its a stabby weapon (play stab animation)
    /// </summary>
    protected bool swinging;

    /// <summary>
    /// true means its a two-handed weapon (play two-handed weapon animation), false means its a one-handed weapon (play one-handed animation)
    /// </summary>
    protected bool twoHanded;

    protected Collider weaponCollider;
    protected GameObject weapon;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        // resetRotation = transform.localRotation;
        // AttackDuration = .25f;
        Damage = 5;
        // weaponCollider = GetComponent<Collider>();
        // print(weaponCollider);
        // weaponCollider.enabled = false;
        // print(weaponCollider);
    }

    public void OnTriggerEnter(Collider other)
    {
        // Help.print("send damage", Damage, other.name);
        if (other.GetComponent<Stats>() != null)
        {
            other.SendMessage("TakeDamage", Damage);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        // base.Update();
        // print(weaponCollider);
    }

    // public override void InitiateAttack()
    // {
    //     base.InitiateAttack();
    // }

    public override void AttackUpdate()
    {
        
    }
    public override void InitiateAttack(GameObject attacker)
	{
        base.InitiateAttack(attacker);
        // weapon.SetActive(true);
        // weaponCollider.enabled = true;
    }

    public override void OnEndAttack()
    {
        //resets position as a placeholder for animation
        // transform.localRotation = resetRotation;
        // weapon.SetActive(false);
        // weaponCollider.enabled = false;
    }

}