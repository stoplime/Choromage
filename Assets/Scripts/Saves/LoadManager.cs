using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using LitJson;
using System.IO;

public class LoadManager : MonoBehaviour {
    private static string TileFileNameJsonPath = "/StreamingAssets/bin/tileFiles.json";
    private Dictionary<SavedObjType, List<ObjectInfo>> savedObjLists = new Dictionary<SavedObjType, List<ObjectInfo>>();

    private CharacterInfo savedPlayer;

    public void Awake () {
        foreach (SavedObjType type in Enum.GetValues(typeof(SavedObjType)))
        {
            savedObjLists[type] = new List<ObjectInfo>();
        }

        if (File.Exists(Application.dataPath + EnvironmentSaver.EnviromentDataPath))
        {
            savedObjLists[SavedObjType.EnvironmentObjects] = JsonMapper.ToObject<List<ObjectInfo>>(File.ReadAllText(Application.dataPath + EnvironmentSaver.EnviromentDataPath));
        }
		if (File.Exists(Application.dataPath + EnvironmentSaver.TileDataPath))
        {
            savedObjLists[SavedObjType.Tiles] = JsonMapper.ToObject<List<ObjectInfo>>(File.ReadAllText(Application.dataPath + EnvironmentSaver.TileDataPath));
        }
		if (File.Exists(Application.dataPath + EnvironmentSaver.NPCsDataPath))
        {
            savedObjLists[SavedObjType.NPCs] = JsonMapper.ToObject<List<ObjectInfo>>(File.ReadAllText(Application.dataPath + EnvironmentSaver.NPCsDataPath));
        }
		if (File.Exists(Application.dataPath + EnvironmentSaver.InitInventoryDataPath))
        {
            savedObjLists[SavedObjType.Inventory] = JsonMapper.ToObject<List<ObjectInfo>>(File.ReadAllText(Application.dataPath + EnvironmentSaver.InitInventoryDataPath));
        }
        // Player info
        if (File.Exists(Application.dataPath + SaveManager.PlayerDataPath))
        {
            savedPlayer = JsonMapper.ToObject<CharacterInfo>(File.ReadAllText(Application.dataPath + SaveManager.PlayerDataPath));
            LoadPlayerInfo();
        }
        // Quests
        if (File.Exists(Application.dataPath + SaveManager.QuestDataPath))
        {
            List<Quest> QuestData = JsonMapper.ToObject<List<Quest>>(File.ReadAllText(Application.dataPath + SaveManager.QuestDataPath));
            if (QuestData != null)
            {
                QuestManager.ListOfQuests = QuestData;
            }
            Quest q = QuestManager.GetQuestByID("ThorinIntroQuest");
            if(q.ID != null)
            {
                GameManager.InTutorial = !q.Completed;
            }
        }
        // Dialogue Variables
        if (File.Exists(Application.dataPath + SaveManager.DialogueDataPath))
        {
            DialogueSaver dialogueSaver = JsonMapper.ToObject<DialogueSaver>(File.ReadAllText(Application.dataPath + SaveManager.DialogueDataPath));
            if (dialogueSaver != null)
            {
                dialogueSaver.LoadToDialogBox();
            }
        }
        // Unlocked Spells
        // if (File.Exists(Application.dataPath + SpellManager.InitializedUnlockedSpellsPath) && !File.Exists(Application.dataPath + SpellManager.UnlockedSpellsPath))
        // {
        //     JsonData spellsData = JsonMapper.ToJson(JsonMapper.ToObject(File.ReadAllText(Application.dataPath + SpellManager.InitializedUnlockedSpellsPath)));
		//     File.WriteAllText(Application.dataPath + SpellManager.UnlockedSpellsPath, spellsData.ToString());
        // }

        if (ChunkManager.TileReference == null)
        {
            ChunkManager.TileReference = new Dictionary<string, TileBase>();
            // string[] allFiles = Directory.GetFiles("Assets/Resources/Tiles/WorldTiles");
            Dictionary<string, string> tileFileNames = new Dictionary<string, string>();
            if (Application.isEditor)
            {
                string[] allFiles = Directory.GetFiles(Application.dataPath + "/Resources/Tiles/WorldTiles");

                for (int i = 0; i < allFiles.Length; i++)
                {
                    if (allFiles[i].Contains(".meta"))
                        continue;
                    string prefabName = Path.GetFileNameWithoutExtension(allFiles[i]);
                    // Help.print(prefabName);
                    string tileFile = "Tiles/WorldTiles/" + prefabName;
                    tileFileNames.Add(tileFile, prefabName);
                }
                // Save the tileFileNames
                JsonData tileFileNameData = JsonMapper.ToJson(tileFileNames);
		        File.WriteAllText(Application.dataPath + TileFileNameJsonPath, tileFileNameData.ToString());
            }
            else
            {
                // Read from json
                if (File.Exists(Application.dataPath + TileFileNameJsonPath))
                {
                    tileFileNames = JsonMapper.ToObject<Dictionary<string, string>>(File.ReadAllText(Application.dataPath + TileFileNameJsonPath));
                }
                else
                {
                    throw new FileNotFoundException("Streaming Assets " + TileFileNameJsonPath + " Path not found.");
                }
            }
            foreach (KeyValuePair<string, string> tileFileName in tileFileNames)
            {
                TileBase tile = Resources.Load<TileBase>(tileFileName.Key);
                ChunkManager.TileReference.Add(tileFileName.Value, tile);
            }
        }
        SpellManager.LoadSpells();
    }

    private void Start()
    {
        MinimapIndicatorManager.UpdateMinimapIndicator();
    }

    public void LoadPlayerInfo()
    {
        GameManager.player.transform.position = new Vector3(savedPlayer.position[0], 0, savedPlayer.position[1]);
        GameObject camera = GameObject.Find("CameraControler");
        camera.transform.position = camera.GetComponent<CameraControls>().GetCameraPosFromPlayer();
        
		PlayerStats ps = GameManager.player.GetComponent<PlayerStats>();
        
        float tempMaxHealth =(float)savedPlayer.stats["MaxHealth"];
        ps.BaseMaxHealth = tempMaxHealth;
        ps.BaseMaxMana = (float)savedPlayer.stats["MaxMana"];

        ps.BaseHealthRegen = (float)savedPlayer.stats["HealthRegen"];
        ps.BaseManaRegen = (float)savedPlayer.stats["ManaRegen"];

        float tempHealth = (float)savedPlayer.stats["Health"];
        if (tempHealth < tempMaxHealth/5)
        {
            tempHealth += tempMaxHealth/5;
        }
        ps.Health = tempHealth;
        ps.Mana = (float)savedPlayer.stats["Mana"];
        
        ps.Gold = (int)savedPlayer.stats["Gold"];

        GameManager.player.GetComponent<PlayerUIScript>().LeveledUpdate();
    }

    public List<ObjectInfo> GetSavedObjFromChunkPos(Vector2 pos, SavedObjType type)
    {
        List<ObjectInfo> newListofObjects = new List<ObjectInfo>();
        for (int i = 0; i < savedObjLists[type].Count; i++)
        {
            ObjectInfo data = savedObjLists[type][i];
            if (data.ChunkPos[0] == (int)pos.x && data.ChunkPos[1] == (int)pos.y)
            {
                newListofObjects.Add(data);
            }
        }
        return newListofObjects;
    }
}
