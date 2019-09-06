using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour {
    
    
    private void Start()
    {
        // Help.print("creating chunks", ChunkManager.RenderDistance);
		ChunkManager.PlayerChunkCoord = new Vector2Int((int)Mathf.Floor(GameManager.player.transform.position.x / ChunkManager.Chunk2GlobalRate),
                                                       (int)Mathf.Floor(GameManager.player.transform.position.z / ChunkManager.Chunk2GlobalRate));
        ChunkManager.Chunks = new List<Chunk>();
        for (int i = -ChunkManager.RenderDistance; i <= ChunkManager.RenderDistance; i++)
		{
			for (int j = -ChunkManager.RenderDistance; j <= ChunkManager.RenderDistance; j++)
			{
				Vector2Int iterPos = new Vector2Int(i, j);
				if ((iterPos - new Vector2Int(0, 0)).magnitude < ChunkManager.RenderDistance)
				{
                    CreateChunk(ChunkManager.PlayerChunkCoord + iterPos);
				}
			}
		}
    }
    
    private void Update()
    {
        Vector2Int newPlayerChunkPos = new Vector2Int((int)Mathf.Floor(GameManager.player.transform.position.x / ChunkManager.Chunk2GlobalRate),
                                                      (int)Mathf.Floor(GameManager.player.transform.position.z / ChunkManager.Chunk2GlobalRate));

		if (!newPlayerChunkPos.Equals(ChunkManager.PlayerChunkCoord))
		{
			ChunkManager.PlayerChunkCoord = newPlayerChunkPos;
			// update the chucks
			// Delete distant chunks
			for (int i = ChunkManager.Chunks.Count-1; i >= 0; i--)
			{
				if ((ChunkManager.Chunks[i].ChunkPos-ChunkManager.PlayerChunkCoord).magnitude > ChunkManager.RenderDistance * ChunkManager.TrailingFactor)
				{
					Object.Destroy(ChunkManager.Chunks[i]);
					ChunkManager.Chunks.RemoveAt(i);
				}
			}
			// Add new chunks
			for (int i = -ChunkManager.RenderDistance; i <= ChunkManager.RenderDistance; i++)
			{
				for (int j = -ChunkManager.RenderDistance; j <= ChunkManager.RenderDistance; j++)
				{
					bool foundChunk = false;
					Vector2Int iterPos = new Vector2Int(i, j);
					for (int k = 0; k < ChunkManager.Chunks.Count; k++)
					{
						if (ChunkManager.Chunks[k].ChunkPos.Equals(ChunkManager.PlayerChunkCoord + iterPos))
						{
							foundChunk = true;
							break;
						}
					}
					if (!foundChunk && (iterPos - new Vector2Int(0, 0)).magnitude < ChunkManager.RenderDistance)
					{
                        CreateChunk(ChunkManager.PlayerChunkCoord + iterPos);
					}
				}
			}
		}
    }

    private void CreateChunk(Vector2Int pos)
    {
        // print(pos);
        Chunk newChunk = MonoBehaviour.Instantiate<Chunk>(Resources.Load<Chunk>("Chunk"), GameObject.Find("ChunkController").transform);
        // print(newChunk.gameObject);
        newChunk.ChunkPos = pos;
        ChunkManager.Chunks.Add(newChunk);
    }
}