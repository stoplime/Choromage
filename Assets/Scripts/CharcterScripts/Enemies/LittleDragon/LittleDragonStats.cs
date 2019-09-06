using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDragonStats : EnemyStats {

    // // Use this for initialization
    public override void Start () {
        base.Start();
        GetComponent<ElementalResistances>().OverrideElement(Element.fire, 0f);
    }

    // // Update is called once per frame
    public override void Update () {
        base.Update();
        if (GetComponent<ElementalResistances>().ElementalBuffsDebuffs.Count == 0)
        { 
            GetComponent<ElementalResistances>().OverrideElement(Element.fire, 0f);
        }
    }
    // public override void SetBuffsDebuffs()
    // { 
	// 	elementalBuffsDebuffs.Add(Element.neutral, 1);
    //     elementalBuffsDebuffs.Add(Element.water, 1);
    //     elementalBuffsDebuffs.Add(Element.earth, 1);
    //     elementalBuffsDebuffs.Add(Element.fire, 0);
    //     elementalBuffsDebuffs.Add(Element.earth, 1);
	// }
}
