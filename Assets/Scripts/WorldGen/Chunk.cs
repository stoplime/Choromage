using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour {

	public Vector2Int ChunkPos
    {
        get
        {
            return chunkPos;
        }
        set
        {
            chunkPos = value;
			transform.position = new Vector3(chunkPos.x * ChunkManager.Chunk2GlobalRate, 0, chunkPos.y * ChunkManager.Chunk2GlobalRate);
        }
    }

	private LoadManager loader;
    private Vector2Int chunkPos;

	private GameObject tilesTilemap;
	private GameObject NPCsRoot;
	private List<GameObject> envObjects;
	private List<GameObject> NPCObjects;
	private List<GameObject> InventoryObjects;

	// public void Awake()
	// {

	// }

    public void Start()
	{
		GameObject grid = GameObject.Find("Grid");
		Vector3 pos = new Vector3(
			chunkPos.x * ChunkManager.Chunk2GlobalRate,
			0,
			chunkPos.y * ChunkManager.Chunk2GlobalRate
		);
		tilesTilemap = Instantiate(Resources.Load<GameObject>("Tiles/EmptyTilemap"), pos, Quaternion.identity, grid.transform);
		tilesTilemap.name = "Chunk_" + chunkPos.ToString();

		NPCsRoot = GameObject.Find("NPCs");
		loader = GameObject.Find("GameManager").GetComponent<LoadManager>();
		InitializeTiles();
		InstantiateEnvironmentObjects();
		InstantiateNPCs();
		InitialzeInventories();
		InitializeEnemies();
	}

	public void InitialzeInventories()
	{
		InventoryObjects = new List<GameObject>();
		List<ObjectInfo> inventories = loader.GetSavedObjFromChunkPos(new Vector2(chunkPos.x, chunkPos.y), SavedObjType.Inventory);

		for (int i = 0; i < inventories.Count; i++)
		{
			ObjectInfo data = inventories[i];
			if (data.Prefab == "")
				continue;
			
			Vector3 pos = new Vector3(
				(float)data.LocalPos[0] + transform.position.x,
				0.0f,
				(float)data.LocalPos[1] + transform.position.z
			);

			Quaternion rotation = Quaternion.Euler((float)data.Rotation[0], (float)data.Rotation[1], (float)data.Rotation[2]);

			GameObject inventoryObj = Instantiate(Resources.Load<GameObject>(data.Prefab), pos, rotation, transform);
			inventoryObj.name = data.ObjectName;
			InventoryObjects.Add(inventoryObj);

			Inventory invClass = inventoryObj.GetComponentInChildren<Inventory>();
			if (invClass == null)
			{
				Help.print("Inventory at", pos, "does not have an inventory.");
				continue;
			}
			
			if (InventorySaver.MostRecentInventorySave.ContainsKey(data.ObjectName))
			{
				invClass.Load(InventorySaver.MostRecentInventorySave[data.ObjectName]);
			}
			else
			{
				invClass.Load(new List<ItemSave>(data.Inventory));
			}
		}
	}

	public void InstantiateNPCs()
	{
		NPCObjects = new List<GameObject>();
		List<ObjectInfo> npcs = loader.GetSavedObjFromChunkPos(new Vector2(chunkPos.x, chunkPos.y), SavedObjType.NPCs);

		for (int i = 0; i < npcs.Count; i++)
		{
			ObjectInfo data = npcs[i];
			if (data.Prefab == "")
				continue;
			
			// Help.print("Spawning NPCs", data);

			Vector3 pos = new Vector3(
				(float)data.LocalPos[0] + transform.position.x,
				0.0f,
				(float)data.LocalPos[1] + transform.position.z
			);

			if (data.ObjectName == "DeadGuard")
			{
				pos.y = 0.95f;
			}

			Quaternion rotation = Quaternion.Euler((float)data.Rotation[0], (float)data.Rotation[1], (float)data.Rotation[2]);
			
			GameObject npcObj = Instantiate(Resources.Load<GameObject>(data.Prefab), pos, rotation, NPCsRoot.transform);
			npcObj.name = data.ObjectName;
			NPCObjects.Add(npcObj);
		}
	}

	public void InitializeTiles()
	{
		List<ObjectInfo> envTileInfos = loader.GetSavedObjFromChunkPos(new Vector2(chunkPos.x, chunkPos.y), SavedObjType.Tiles);

		for (int i = 0; i < envTileInfos.Count; i++)
		{
			ObjectInfo data = envTileInfos[i];
			if (data.ObjectName == "null")
				continue;
				
			Vector3Int pos = new Vector3Int(
				(int)Mathf.Floor((float)data.LocalPos[0]) + ChunkManager.ChunkSize/2,
				(int)Mathf.Floor((float)data.LocalPos[1]) + ChunkManager.ChunkSize/2,
				0
			);

			TileBase tile = ChunkManager.TileReference[data.ObjectName];
			tilesTilemap.GetComponent<Tilemap>().SetTile(pos, tile);
		}
		
	}

	public void InstantiateEnvironmentObjects()
	{
		List<ObjectInfo> envObjectInfos = loader.GetSavedObjFromChunkPos(new Vector2(chunkPos.x, chunkPos.y), SavedObjType.EnvironmentObjects);
		envObjects = new List<GameObject>();

		// Help.print(envObjectInfos.Count);
		for (int i = 0; i < envObjectInfos.Count; i++)
		{
			ObjectInfo data = envObjectInfos[i];
			if (data.Prefab == "")
				continue;

			Vector3 pos = new Vector3(
				(float)data.LocalPos[0],
				0.1f,
				(float)data.LocalPos[1]
			);

			Vector3 localScale = new Vector3(
				(float)data.LocalScale[0],
				(float)data.LocalScale[1],
				(float)data.LocalScale[2]
			);
			// Help.print(pos);

			// string cutAsset = data.Prefab.Replace("Assets/Resources/", "");
			// cutAsset = cutAsset.Replace(".prefab", "");
			// Help.print(cutAsset);
			// Help.print(data.Prefab);
			if (Resources.Load<GameObject>(data.Prefab) == null)
			{
				Help.print("Enrivonment Prefab ------", data.Prefab);
				continue;
			}
			GameObject envObj = Instantiate(Resources.Load<GameObject>(data.Prefab), transform);
			if (envObj.tag == "IndoorEnvironment")
			{
				pos.y = 0f;
			}
			envObj.transform.localPosition = pos;
			envObj.transform.localScale = localScale;
			envObj.name = data.ObjectName;

			SpriteRenderer sr = envObj.GetComponentInChildren<SpriteRenderer>();
			if (sr != null)
			{
				sr.sortingOrder = data.SpriteOrder;
			}

			envObjects.Add(envObj);
		}
	}

	public void InitializeEnemies()
	{
		GameObject EnemySpawner = GameObject.Find("EnemySpawner");
		if (EnemySpawner == null)
			return;
		EnemySpawning es = EnemySpawner.GetComponent<EnemySpawning>();
		if (es == null)
			return;
		Vector3 chunkSize = ChunkManager.Chunk2GlobalRate * Vector3.one;
		Vector3 chunkCenter = transform.position + chunkSize/2;
		chunkCenter.y = 0;
		// print(chunkCenter);
		if (!es.IsInSpawnRegion(chunkCenter))
			return;
		// Spawn enemies in
		es.SpawnInChunkEnemies(chunkCenter, chunkSize);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	public void Update()
	{
		// if (transform.childCount == 0)
		// {
		// 	Destroy(gameObject);
		// }
	}

	void OnDestroy()
	{
		foreach (GameObject item in envObjects)
		{
			Destroy(item);
		}
		foreach (GameObject item in NPCObjects)
		{
			Destroy(item);
		}
		foreach (GameObject item in InventoryObjects)
		{
			InventorySaver.DynamicSaveInventoryToSaveJSON(item.name, item.GetComponentInChildren<Inventory>());
			Destroy(item);
		}
		Destroy(tilesTilemap);
	}
}
