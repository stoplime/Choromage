using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalResistances : MonoBehaviour {

/// <summary>
/// key is element, value is added by damage type to determine actual damage. 0 is neutral, negative numbers resistance, positive numbers weak to it.
/// </summary>
/// <typeparam name="Element"></typeparam>
/// <typeparam name="float"></typeparam>
/// <returns></returns>
	private Dictionary<Element, float> elementalBuffsDebuffs = new Dictionary<Element, float>();

/// <summary>
/// key is element, value is multiplied by damage type to determine actual damage.virtual 1 is neutral, 0- complete resistance/negates it, 2 weak/doubles it.
/// </summary>
/// <value></value>
	public Dictionary<Element, float> ElementalBuffsDebuffs
    {
        get { return elementalBuffsDebuffs; }
    }
    // Use this for initialization
    void Start () {
        // elementalBuffsDebuffs.Add(Element.neutral, 1);
        // elementalBuffsDebuffs.Add(Element.water, 1);
        // elementalBuffsDebuffs.Add(Element.earth, 1);
        // elementalBuffsDebuffs.Add(Element.fire, 1);
        // elementalBuffsDebuffs.Add(Element.air, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OverrideElement(Element elem, float resist)
    {
        // elementalBuffsDebuffs.Remove(elem);
        elementalBuffsDebuffs.Add(elem, resist);
    }
}
