using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using LitJson;
using System.IO;
using UnityEditor;

public enum SavedObjType
{
    EnvironmentObjects,
    Tiles,
    NPCs,
    Inventory
}

public class EnvironmentSaver {
    private static string enviromentDataPath = "/StreamingAssets/bin/environment.json";
    private static string tileDataPath = "/StreamingAssets/bin/tiles.json";
    private static string npcsDataPath = "/StreamingAssets/bin/npcs.json";
    private static string initInventoryDataPath = "/StreamingAssets/bin/initialInventory.json";

    private Dictionary<SavedObjType, List<ObjectInfo>> savedObjLists = new Dictionary<SavedObjType, List<ObjectInfo>>();

    public GameObject RootEnvironmentFolder;

    public static string EnviromentDataPath
    {
        get
        {
            return enviromentDataPath;
        }
    }

    public static string TileDataPath
    { 
        get
        {
            return tileDataPath;
        }
    }

    public static string NPCsDataPath
    { 
        get
        {
            return npcsDataPath;
        }
    }

    public static string InitInventoryDataPath
    { 
        get
        {
            return initInventoryDataPath;
        }
    }

    public EnvironmentSaver()
    {
        foreach (SavedObjType type in Enum.GetValues(typeof(SavedObjType)))
        {
            savedObjLists[type] = new List<ObjectInfo>();
        }
    }

    public List<ObjectInfo> GetSaveListByType(SavedObjType type)
    {
        return savedObjLists[type];
    }

    /// <summary>
    /// Find all objects in "NewEnv" and convert them to ObjectInfo datatype
    /// </summary>
    public void FindAllEnvironmentObjects()
    {
        RootEnvironmentFolder = GameObject.Find("NewEnv");
        int numOfEnvObjs = RootEnvironmentFolder.transform.childCount;
        for (int i = 0; i < numOfEnvObjs; i++)
        {
            Transform rootChild = RootEnvironmentFolder.transform.GetChild(i);
            GameObject childGameObject = rootChild.gameObject;
            // check layer is of environment
            if (childGameObject.layer != 9 && childGameObject.layer != 15)
                continue;

            // create ObjectInfo
            ObjectInfo data = new ObjectInfo();
            
            // Get the path of the prefab
            EnvironmentFade ef = rootChild.GetComponentInChildren<EnvironmentFade>();
            if (!ef)
                continue;
            data.Prefab = ef.resourcePathway;
            
            // get the object's positions
            data.ChunkPos = new int[2];
            data.ChunkPos[0] = (int)Mathf.Floor(rootChild.position.x / ChunkManager.Chunk2GlobalRate);
            data.ChunkPos[1] = (int)Mathf.Floor(rootChild.position.z / ChunkManager.Chunk2GlobalRate);

            data.LocalPos = new double[2];
            data.LocalPos[0] = rootChild.position.x - data.ChunkPos[0] * ChunkManager.Chunk2GlobalRate;
            data.LocalPos[1] = rootChild.position.z - data.ChunkPos[1] * ChunkManager.Chunk2GlobalRate;

            data.LocalScale = new double[3];
            data.LocalScale[0] = rootChild.localScale.x;
            data.LocalScale[1] = rootChild.localScale.y;
            data.LocalScale[2] = rootChild.localScale.z;

            data.Id = i;

            data.ObjectName = childGameObject.name;
            // Object prefabObject = PrefabUtility.GetCorrespondingObjectFromSource(childGameObject);
            // Help.print(prefabObject);
            // data.Prefab = AssetDatabase.GetAssetPath(prefabObject);

            // Get the sprite order
            SpriteRenderer sr = rootChild.GetComponentInChildren<SpriteRenderer>();
            if (sr)
            {
                data.SpriteOrder = sr.sortingOrder;
            }

            // Add the objectdata to the list
            savedObjLists[SavedObjType.EnvironmentObjects].Add(data);
        }
    }

    /// <summary>
    /// Finds all the tiles in "GroundTiles" and saves them in ObjectInfo datatype.
    /// </summary>
    public void FindTiles()
    {
        Tilemap GroundTileMap = GameObject.Find("GroundTiles").GetComponent<Tilemap>();
        GroundTileMap.CompressBounds();

        BoundsInt bounds = GroundTileMap.cellBounds;
        Help.print(bounds);
        TileBase[] allTiles = GroundTileMap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                int i = x + y * bounds.size.x;
                TileBase tile = allTiles[i];
                Vector2 TilePos = new Vector2(x + bounds.position.x, y + bounds.position.y);
                // Help.print(x, bounds.position.x, y, bounds.position.y, TilePos);
                TilePos *= 10;

                ObjectInfo data = new ObjectInfo();
                data.Id = i;
                data.ChunkPos = new int[2];
                data.ChunkPos[0] = (int)Mathf.Floor(TilePos.x / ChunkManager.Chunk2GlobalRate);
                data.ChunkPos[1] = (int)Mathf.Floor(TilePos.y / ChunkManager.Chunk2GlobalRate);

                data.LocalPos = new double[2];
                data.LocalPos[0] = TilePos.x - data.ChunkPos[0] * ChunkManager.Chunk2GlobalRate;
                data.LocalPos[1] = TilePos.y - data.ChunkPos[1] * ChunkManager.Chunk2GlobalRate;

                if (tile != null) {
                    data.ObjectName = tile.name;
                    // Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                } else {
                    data.ObjectName = "null";
                    // Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
                savedObjLists[SavedObjType.Tiles].Add(data);
            }
        }
    }

    /// <summary>
    /// Finds all the the NPCs and saves them in ObjectInfo datatype.
    /// </summary>
    public void FindNPCs()
    {
        Transform NPCs = GameObject.Find("NPCs").transform;
        string prefabPath = "Characters/NPCs/Oprima/";

        for (int i = 0; i < NPCs.childCount; i++)
        {
            Transform npc = NPCs.GetChild(i);

            ObjectInfo data = new ObjectInfo();
            
            data.ChunkPos = new int[2];
            data.ChunkPos[0] = (int)Mathf.Floor(npc.position.x / ChunkManager.Chunk2GlobalRate);
            data.ChunkPos[1] = (int)Mathf.Floor(npc.position.z / ChunkManager.Chunk2GlobalRate);

            data.LocalPos = new double[2];
            data.LocalPos[0] = npc.position.x - data.ChunkPos[0] * ChunkManager.Chunk2GlobalRate;
            data.LocalPos[1] = npc.position.z - data.ChunkPos[1] * ChunkManager.Chunk2GlobalRate;

            data.Rotation = new double[3];
            data.Rotation[0] = npc.rotation.eulerAngles.x;
            data.Rotation[1] = npc.rotation.eulerAngles.y;
            data.Rotation[2] = npc.rotation.eulerAngles.z;

            data.Id = i;

            data.ObjectName = npc.gameObject.name;
            // Get the path of the prefab
            data.Prefab = prefabPath + data.ObjectName;

            savedObjLists[SavedObjType.NPCs].Add(data);
        }
    }

    public void FindInventories()
    {
        Transform worldInventories = GameObject.Find("World Inventories").transform;
        for (int i = 0; i < worldInventories.childCount; i++)
        {
            Transform chest = worldInventories.GetChild(i);

            ObjectInfo data = new ObjectInfo();

            data.ChunkPos = new int[2];
            data.ChunkPos[0] = (int)Mathf.Floor(chest.position.x / ChunkManager.Chunk2GlobalRate);
            data.ChunkPos[1] = (int)Mathf.Floor(chest.position.z / ChunkManager.Chunk2GlobalRate);

            data.LocalPos = new double[2];
            data.LocalPos[0] = chest.position.x - data.ChunkPos[0] * ChunkManager.Chunk2GlobalRate;
            data.LocalPos[1] = chest.position.z - data.ChunkPos[1] * ChunkManager.Chunk2GlobalRate;

            data.Rotation = new double[3];
            data.Rotation[0] = chest.rotation.eulerAngles.x;
            data.Rotation[1] = chest.rotation.eulerAngles.y;
            data.Rotation[2] = chest.rotation.eulerAngles.z;

            data.Id = i;

            data.ObjectName = chest.gameObject.name;

            data.Prefab = chest.GetComponent<PrefabPath>().Path;

            data.Inventory = chest.GetComponentInChildren<Inventory>().Save().ToArray();

            savedObjLists[SavedObjType.Inventory].Add(data);
        }
    }
    
    public void SaveEnvironmentData()
	{
        FindAllEnvironmentObjects();

		JsonData enviromentData = JsonMapper.ToJson(savedObjLists[SavedObjType.EnvironmentObjects]);
		File.WriteAllText(Application.dataPath + enviromentDataPath, enviromentData.ToString());
	}

    public void SaveTileData()
	{
        FindTiles();
        
		JsonData tileData = JsonMapper.ToJson(savedObjLists[SavedObjType.Tiles]);
		File.WriteAllText(Application.dataPath + tileDataPath, tileData.ToString());
	}

    public void SaveNPCsData()
	{
        FindNPCs();
        
		JsonData npcsData = JsonMapper.ToJson(savedObjLists[SavedObjType.NPCs]);
		File.WriteAllText(Application.dataPath + npcsDataPath, npcsData.ToString());
	}

    public void SaveInitInventoryData()
	{
        FindInventories();
        
		JsonData initInventoryData = JsonMapper.ToJson(savedObjLists[SavedObjType.Inventory]);
		File.WriteAllText(Application.dataPath + initInventoryDataPath, initInventoryData.ToString());
	}
}