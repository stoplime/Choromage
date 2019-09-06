using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BossEnemy
{
	public Vector3 SpawnPos;
    public GameObject Prefab;
    private bool slain;
    private bool exists;
    public bool Exists { get => exists; set => exists = value; }
    public bool Slain { get => slain; set => slain = value; }

    // public float Diameter;
    // public float X;
    // public float Z;

    public BossEnemy(float x, float z, GameObject prefab)
	{
        this.SpawnPos = new Vector3(x,0,z);
        // this.X = x;
        // this.Z = z;
        // this.Diameter = diameter;
        this.Prefab = prefab;
        exists = false;
        slain = false;
    }
    public void GetSlain()
    {
        slain = true;
    }

}