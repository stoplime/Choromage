using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfStats : EnemyStats {

	// Use this for initialization
	public override void Start () {
		base.Start();
		maxHealth = 100;
        maxMana = 0;
        //bufferDistances.Add("passive", 0);
        SetDistances();
        SetElements();
        //print("wolfy");
        //print(bufferDistances.Count);
        //Help.print(bufferDistances);

    }
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
        if (GetComponent<ElementalResistances>().ElementalBuffsDebuffs.Count == 0)
        {
            SetElements();
        }	
	}
    void SetElements()
    { 
		GetComponent<ElementalResistances>().OverrideElement(Element.neutral, .75f);
		GetComponent<ElementalResistances>().OverrideElement(Element.fire, 1.25f);
		GetComponent<ElementalResistances>().OverrideElement(Element.water, .875f);
	}
    public virtual void SetDistances()
    { 
		bufferDistances.Add("aggressive", 2f);
	}
}
