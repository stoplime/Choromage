using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBubble : SplashObject
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
        //print(allWithinRange.Count);
        foreach (Collider c in allWithinRange)
        {
            //Help.print("healing", Damage * Time.deltaTime, c.name);
            c.SendMessage("HealDamage", Damage * Time.deltaTime);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        //if other collider has a health stat (the HealthScript) meaning they can take damage
        if (other.GetComponent<Stats>() != null)
        {
            if (CheckIfFriend(other))
            {
                allWithinRange.Add(other);
            }
        }
        else if(other.GetComponent<ChargeStaff>() != null)
        {
            allWithinRange.Add(other);
        }
    }
}