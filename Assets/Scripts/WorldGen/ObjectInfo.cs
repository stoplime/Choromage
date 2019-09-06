using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo {
    public int Id;
    public string ObjectName;
    public string Prefab;
    public int SpriteOrder;
    public double[] LocalPos;
    public int[] ChunkPos;
    public double[] Rotation;
    public double[] LocalScale;
    public ItemSave[] Inventory;
}