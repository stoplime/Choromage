using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {
	public static Vector3 offset;

	private static Item item;
	private static string data;
	private static GameObject tooltip;

	void Start()
	{
		offset = new Vector3(0, -20, 0);
		tooltip = GameObject.Find("Tooltip");
		tooltip.SetActive(false);
	}

	void Update()
	{
        //print("TODO: Check to see if enclosing within if statement messes with anything");
        if (tooltip != null)
        {
            if (tooltip.activeSelf)
            {
                tooltip.transform.position = Input.mousePosition + offset;
            }
            if (!TopMenu.isOpen)
            {
                Deactivate();
            }
        }
    }

	public static void Activate(Item itemRef)
	{
		item = itemRef;
		ConstructDataString();
		tooltip.SetActive(true);
	}

	public static void Deactivate()
	{
		tooltip.SetActive(false);
	}

	public static void ConstructDataString()
	{
		// Title color
		data = "<color=#0000ff>";
			data += "<b>";
				data += item.Name;
			data += "</b>";
		data += "</color>";
		data += "\n";
		
		data += item.Description;
        if (item.EID != null)
        {
            data += item.EID.ExtraItemDescription();
        }
        tooltip.GetComponentInChildren<Text>().text = data;
	}
}
