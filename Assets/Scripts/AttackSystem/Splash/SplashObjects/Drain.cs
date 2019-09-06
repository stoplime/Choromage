using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : SplashObject
{
    //TODO: clean this method up (possibly separate some parts) and adhere to the samish style as the other SplashObject scripts~~~
    public override void SetVariables(Vector3 targetP, GameObject caster)
    {
        base.SetVariables(targetP, caster);
        lingeringDuration = 1;
        maxDiameter = 7.5f;
        //targetPos = targetP;
        growth = maxDiameter / lingeringDuration;
    }
    public override void CalculateDiameterAndDuration()
    { 
        //maxDiameter = 7.5f;
        //targetPos = targetP;
        growth = maxDiameter / lingeringDuration;
    }

    public override void HurtWithinRange()
    {
        int numEnemies = 0;
        //print(allWithinRange.Count);
        foreach (Collider c in allWithinRange)
        {
            Help.print("draining", Damage * Time.deltaTime, c.name);
            c.SendMessage("TakeDamage", Damage * Time.deltaTime);
            if (c.gameObject.GetComponent<Stats>() != null)
            {
                numEnemies++;
            }
        }
        foreach (Collider c in casterAndFriends)
        {
            if (c.gameObject.GetComponent<Stats>() != null)
            {
                c.SendMessage("RegainMana", Damage * Time.deltaTime* numEnemies);
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        //if other collider has a health stat (the HealthScript) meaning they can take damage
        if (other.GetComponent<Stats>() != null)
        {
            if (!CheckIfFriend(other))
            {
                allWithinRange.Add(other);
            }
        }
        // else if(other.GetComponent<ChargeStaff>() != null)
        // {
        //     allWithinRange.Add(other);
        // }
    }
}