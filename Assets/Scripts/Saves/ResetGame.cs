using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public static class ResetGame {
    public static Dictionary<string, string> SaveJsonPaths = new Dictionary<string, string>();
    public static Dictionary<string, JsonData> AllSaveJsons = new Dictionary<string, JsonData>();
    public static JsonData InitializedUnlockedSpells;
    
    public static void Initialize()
    {
        SaveJsonPaths["PlayerInfo"] = SaveManager.PlayerDataPath;
        SaveJsonPaths["inventory"] = InventorySaver.InventoryJSONPath;
        SaveJsonPaths["UnlockedSpells"] = SpellManager.UnlockedSpellsPath;
        SaveJsonPaths["DialogueProgress"] = SaveManager.DialogueDataPath;
        SaveJsonPaths["QuestProgress"] = SaveManager.QuestDataPath;

        foreach (KeyValuePair<string, string> json in SaveJsonPaths)
        {
            if (File.Exists(Application.dataPath + json.Value))
            {
                AllSaveJsons[json.Key] = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + json.Value));
            }
        }
        if (File.Exists(Application.dataPath + SpellManager.InitializedUnlockedSpellsPath))
        {
            InitializedUnlockedSpells = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + SpellManager.InitializedUnlockedSpellsPath));
        }
    }

    public static void EmptySave()
    {
        foreach (KeyValuePair<string, string> json in SaveJsonPaths)
        {
		    File.Delete(Application.dataPath + json.Value);
        }
        if (InitializedUnlockedSpells != null)
        {
            File.WriteAllText(Application.dataPath + SaveJsonPaths["UnlockedSpells"], InitializedUnlockedSpells.ToJson().ToString());
        }
    }
    
}