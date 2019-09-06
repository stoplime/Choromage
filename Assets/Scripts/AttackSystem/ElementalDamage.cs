using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalDamage: MonoBehaviour {

	/// <summary>
    /// the float represents the damage per elemental type
    /// </summary>
    //protected Dictionary <Element, float> elements = new Dictionary<Element, float>();
	private Dictionary<Element, float> elements = new Dictionary<Element, float>();

	public Dictionary<Element, float> Elements
    {
        get { return elements; }
    }
    // Use this for initialization
    public void AddElement(Element elem, float damage)
    {
        elements.Add(elem, damage);
    }
    public override string  ToString()
    {
        string temp = "";
        foreach (KeyValuePair<Element, float> elem in elements)
        {
            temp += elem.Key + ": " + elem.Value + "\t";
        }
        return temp;
    }
}
