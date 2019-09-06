using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeSpike : SplashObject
{
    //TODO: clean this method up (possibly separate some parts) and adhere to the samish style as the other SplashObject scripts~~~
    public override void SetVariables(Vector3 targetP, GameObject caster)
    {
        base.SetVariables(targetP, caster);
        lingeringDuration = 1;
        maxDiameter = 3f;
        //targetPos = targetP;
        growth = (maxDiameter* 7.5f)/ lingeringDuration;
        speed = 2 * Vector3.Distance(targetPos, transform.position) / lingeringDuration;
    }
    public override void CalculateDiameterAndDuration()
    { 
        growth = (maxDiameter* 7.5f)/ lingeringDuration;
        speed = 2 * Vector3.Distance(targetPos, transform.position) / lingeringDuration;
    }
    public override void GrowShrink()
    {
        if (maxDiameter > transform.localScale.y)
        {
            transform.localScale += (new Vector3(0, growth, 0) * Time.deltaTime);
        }
    }

    public override void Move()
    {
        //transform.position += (transform.forward * speed * Time.deltaTime);
        if (transform.position.y > targetPos.y)
        {
            transform.position -= new Vector3(0, Time.deltaTime, 0);
        }
    }

    // public override void HurtWithinRange()
    // {
    //     //print("quakeHurty");
    //     foreach (Collider c in allWithinRange)
    //     {
    //         //Help.print("send damage", Damage * Time.deltaTime, c.name);
    //         c.SendMessage("TakeDamage", Damage * Time.deltaTime);
    //     }
    // }
}