using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemManager : MonoBehaviour {
	/// <summary>
	/// The list of items that exist in the game
	/// </summary>
	/// <typeparam name="Item"></typeparam>
	/// <returns></returns>
	private static List<Item> itemLibrary = new List<Item>();
	// might not be necissary to have it as a member variable
	private JsonData itemData;


	public static int GetItemLibCount()
	{
		return itemLibrary.Count;
	}

	public static Item GetItemLib(int id)
	{
		if (id >= 0 && id < itemLibrary.Count)
		{
			return itemLibrary[id];
		}
		return new Item();
	}

	public static Item GetItemLibByTitle(string title)
	{
		for (int i = 0; i < itemLibrary.Count; i++)
		{
			if (itemLibrary[i].Name == title)
			{
				return itemLibrary[i];
			}
		}
		return new Item();
	}

	/// <summary>
	/// Loads in the JSON for the item list
	/// To add more variables, add it to the Item constructor and pass it in here
	/// </summary>
	/// <param name="itemData"></param>
	private static void CreateLibrary(JsonData itemData)
	{
		for (int i = 0; i < itemData.Count; i++)
		{
			IDictionary dictData = itemData[i] as IDictionary;
			ExtraItemData_JSON eid_json = null;
			ExtraItemData eid = null;
			if (dictData.Contains("ied"))
			{
				eid_json = JsonMapper.ToObject<ExtraItemData_JSON>(itemData[i]["ied"].ToJson());
				eid = new ExtraItemData();
				eid.JSON_To_ExtraItemData(eid_json);
			}
			itemLibrary.Add(new Item(i,
									 itemData[i]["Name"].ToString(),
									 (int)itemData[i]["Value"],
									 (int)itemData[i]["Price"],
									 (int)itemData[i]["MaxStackSize"],
									 itemData[i]["Description"].ToString(),
									 itemData[i]["Slug"].ToString(),
									 itemData[i]["EquipTag"].ToString(),
									 eid
									 ));
            // if (itemLibrary[itemLibrary.Count - 1].EID != null)
            // {
            //     itemLibrary[itemLibrary.Count - 1].EID.ConvertStringsToVariables();
            // }
        }
	}

	void Start () {
		itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/bin/ItemDatabase.json"));
		CreateLibrary(itemData);
	}
}
