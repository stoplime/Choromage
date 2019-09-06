using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public static class InventorySaver {
    public static string InventoryJSONPath = "/StreamingAssets/save/inventory.json";

    public static Dictionary<string, List<ItemSave>> MostRecentInventorySave = new Dictionary<string, List<ItemSave>>();

    public static void DynamicSaveInventoryToSaveJSON(string name, Inventory inv)
    {
        json_to_class();
        if (MostRecentInventorySave == null)
        {
            MostRecentInventorySave = new Dictionary<string, List<ItemSave>>();
        }
        List<ItemSave> invSave = inv.Save();
        if (!MostRecentInventorySave.ContainsKey(name))
        {
            MostRecentInventorySave.Add(name, invSave);
        }
        else
        {
            MostRecentInventorySave[name] = invSave;
        }
        class_to_json();
    }

    private static void json_to_class()
    {
        if (File.Exists(Application.dataPath + InventoryJSONPath))
        {
            MostRecentInventorySave = JsonMapper.ToObject<Dictionary<string, List<ItemSave>>>(File.ReadAllText(Application.dataPath + InventoryJSONPath));
        }
    }

    private static void class_to_json()
    {
        JsonData jsondata = JsonMapper.ToJson(MostRecentInventorySave);
		File.WriteAllText(Application.dataPath + InventoryJSONPath, jsondata.ToString());
    }

    public static void LoadPlayerInventories()
    {
        MostRecentInventorySave = new Dictionary<string, List<ItemSave>>();
        json_to_class();
        if (MostRecentInventorySave == null)
        {
            MostRecentInventorySave = new Dictionary<string, List<ItemSave>>();
        }
        if (MostRecentInventorySave.ContainsKey("Player Inventory"))
        {
            GameManager.playerInventory.Load(MostRecentInventorySave["Player Inventory"]);
        }
        if (MostRecentInventorySave.ContainsKey("Player Equipment"))
        {
            GameManager.playerEquipments.Load(MostRecentInventorySave["Player Equipment"]);
        }
    }

    public static void SavePlayerInventories()
    {
        DynamicSaveInventoryToSaveJSON("Player Inventory", GameManager.playerInventory);
        DynamicSaveInventoryToSaveJSON("Player Equipment", GameManager.playerEquipments);
    }

    public static void ClearSavedInventories()
    {
        MostRecentInventorySave = new Dictionary<string, List<ItemSave>>();
        class_to_json();
    }
}
