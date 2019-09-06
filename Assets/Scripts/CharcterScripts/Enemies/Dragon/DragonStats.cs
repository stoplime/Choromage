using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStats : EnemyStats
{

    // // Use this for initialization
    public override void Start()
    {
        InitializeDistances();
        base.Start();
        InitializeElementalResistances();
        maxHealth = 50;
        health = 50;
    }
    void InitializeDistances()
    {
        bufferDistances.Add("aggressive", 15f);
        sightDistances.Add("aggressive", 22.5f);
        sightDistances.Add("guarded", 25f);
        sightDistances.Add("passive", 18.75f);
        moveSpeeds.Add("aggressive", 4.5f);
        moveSpeeds.Add("guarded", 4f);
    }
    // // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (GetComponent<ElementalResistances>().ElementalBuffsDebuffs.Count == 0)
        {
            InitializeElementalResistances();
        }
    }
    void InitializeElementalResistances()
    { 
        GetComponent<ElementalResistances>().OverrideElement(Element.fire, 0);
        GetComponent<ElementalResistances>().OverrideElement(Element.earth, .9f);
        GetComponent<ElementalResistances>().OverrideElement(Element.water, 1.5f);
    }

    //     public enum StateOfMindEnum
    // {
    // 	passive,
    // 	guarded,
    // 	aggressive,
    // 	defensive,
    // 	dead
    // }
}