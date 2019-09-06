using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class ChunkManager {
    public static int ChunkSize
    {
        get
        {
            return chunkSize;
        }

        set
        {
            chunkSize = value;
        }
    }
    public static int RenderDistance
    {
        get
        {
            return renderDistance;
        }

        set
        {
            renderDistance = value;
        }
    }
    public static Vector2Int PlayerChunkCoord
    {
        get
        {
            return playerChunkCoord;
        }

        set
        {
            playerChunkCoord = value;
        }
    }
    public static float TrailingFactor
    {
        get
        {
            return trailingFactor;
        }

        set
        {
            trailingFactor = value;
        }
    }

    public static float TileSize
    {
        get
        {
            return tileSize;
        }

        set
        {
            tileSize = value;
        }
    }

    public static float Chunk2GlobalRate
    {
        get
        {
            return tileSize * chunkSize;
        }
    }

    public static List<Chunk> Chunks;

    public static Dictionary<string, TileBase> TileReference;

    private static float tileSize = 5f;
    private static int chunkSize = 8;
    private static int renderDistance = 3;
    private static float trailingFactor = 1.5f;
    private static Vector2Int playerChunkCoord = new Vector2Int(0, 0);

    public static bool ChunkExists(Vector2Int chunkCoord)
    {
        foreach (Chunk chunk in Chunks)
        {
            if (chunk.ChunkPos == chunkCoord)
            {
                return true;
            }
        }
        return false;
    }
}
