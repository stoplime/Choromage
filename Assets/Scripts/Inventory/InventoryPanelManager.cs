using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelManager : MonoBehaviour {
	/// <summary>
	/// List of all the different inventory pannels there are
	/// </summary>
	/// <typeparam name="string"></typeparam>
	/// <typeparam name="GameObject"></typeparam>
	/// <returns></returns>
	private static Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();
	
	public static Inventory CurrentOtherInventory = null;

	private void Awake()
	{
		foreach (Transform panel in this.transform)
		{
			if (panels.ContainsKey(panel.name))
			{
				panels.Remove(panel.name);
			}
			panels.Add(panel.name, panel.gameObject);
		}
	}

	public static List<string> GetPanelNames()
	{
		return new List<string>(panels.Keys);
	}

	public static void EnablePanel(string name)
	{
		panels[name].SetActive(true);
		if (CurrentOtherInventory != null && name == "OtherInventoryPanel")
		{
			CurrentOtherInventory.OnEnableInventory();
		}
	}

	public static void DisablePanel(string name)
	{
		if (CurrentOtherInventory != null && name == "OtherInventoryPanel")
		{
			CurrentOtherInventory.OnDisableInventory();
			CurrentOtherInventory = null;
		}
		panels[name].SetActive(false);
	}
}
