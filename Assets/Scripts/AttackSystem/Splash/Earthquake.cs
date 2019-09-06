using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : Splash
{
    public override bool NeedsTarget
    {
        get
        {
            return true;
        }
    }

    // Use this for initialization
    //public override void Start()
    // {
    //     base.Start();
    //     coolDown = 2f;
    //     //manaCost = 5;
    // }

    // Update is called once per frame
    // public override void Update()
    // {
    //     base.Update();
    // }

    public override void OnEndAttack()
    {
        //base.OnEndAttack();
        //GameObject quake = Instantiate(splashPrefab, startPos, transform.rotation);
        GameObject quake = Instantiate(splashPrefab, startPos, Quaternion.identity);
        //Help.print("quke:" +damage,"base"+base.damage);
        quake.GetComponent<QuakeSpike>().SetVariables(targetPos, Caster, lingeringDuration,maxDiameter,elements,base.damage);
        base.BaseOnEndAttack();
    }
    public override void InitiateAttack(GameObject attacker)
    {
        base.InitiateAttack(attacker);
        targetPos += new Vector3 (0,1,0);
        startPos = new Vector3(targetPos.x, -1, targetPos.z);
    }
    
}
