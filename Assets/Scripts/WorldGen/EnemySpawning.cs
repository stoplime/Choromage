using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyName
{
	public string Name;
	public int Count;

    public float Diameter;
    public EnemyName(string name, int count, float diameter)
	{
		this.Name = name;
		this.Count = count;
        this.Diameter = diameter;
    }
}
public class EnemySpawning : MonoBehaviour {
    private static Vector3 center = new Vector3(-4.871643f, 0f, 7.985413f);
    private static Vector3 size = new Vector3(373.5927f, 1f, 379.5157f);
    private static float MaxEnemiesPerChunk = 1.2f;
	// public BoxCollider SpawningRegion;
    //public Transform spawnArea;
    public List<EnemyName> Enemies;
    bool collide;
    // public string bossName;
    // public Vector3 spawnPos;
    public List<string> Bosses;
    private int totalEnemies = 0;
    private void Start() {
        foreach (string boss in Bosses)
        {
            EnemyManager.InstantiateBosses(boss);
        }
        for (int i = 0; i < Enemies.Count; i++)
		{
            totalEnemies += Enemies[i].Count;
		}
	}
    public List<GameObject> SpawnInChunkEnemies(Vector3 chunkCenter, Vector3 chunkSize)
    {
        List<GameObject> SpawnedEnemies = new List<GameObject>();
        EnemyName spawningEnemy;
        int minMaxEnemiesPerChunk = (int)Mathf.Floor(MaxEnemiesPerChunk);
        int probabilityExtra = (UnityEngine.Random.Range(0f, 1f) < MaxEnemiesPerChunk-minMaxEnemiesPerChunk)?1:0;
        for (int i = 0; i < minMaxEnemiesPerChunk+probabilityExtra; i++)
        {
            spawningEnemy = Enemies[UnityEngine.Random.Range(0, Enemies.Count)];
            Vector3 randomPos = RandomSpawnPosition(spawningEnemy.Diameter, chunkCenter, chunkSize, 1);
            if (randomPos.x == float.NegativeInfinity)
            {
                continue;
            }
            
			GameObject currentGameObject = null;
			for (int j = 0; j < EnemyManager.Enemies.Count; j++)
			{
				if (spawningEnemy.Name == EnemyManager.Enemies[j].name)
				{
					currentGameObject = EnemyManager.Enemies[j];
                    break;
				}
			}
            GameObject newEnemy = Instantiate(currentGameObject, randomPos, transform.rotation, GameObject.Find("OtherEnemies").transform);
            newEnemy.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
            SpawnedEnemies.Add(newEnemy);
        }
        return SpawnedEnemies;
    }

    public Vector3 RandomSpawnPosition(float diameter, Vector3 spawnCenter, Vector3 spawnSize, int attempts = 10000)
    {
        if (diameter == 0)
        {
            diameter = 1f;
        }
        Vector3 randomPos = Vector3.negativeInfinity;
        do
        {
            attempts--;
            randomPos = spawnCenter + new Vector3(
                            (UnityEngine.Random.value - 0.5f) * spawnSize.x,
                            0f,
                            (UnityEngine.Random.value - 0.5f) * spawnSize.z
                        );
            Collider[] colliders = Physics.OverlapSphere(randomPos, diameter/2);
            foreach (Collider c in colliders)
            {
                // Help.print(c.gameObject.layer);
                if (c.gameObject.layer == 9 || c.gameObject.layer == 4 || c.gameObject.layer == 12)
                {
                    // Help.print("breaking");
                    randomPos = Vector3.negativeInfinity;
                    break;
                }
            }
        } while (attempts>0);
        return randomPos;
    }

    public bool IsInSpawnRegion(Vector3 point)
    {
        Vector3 min = center + transform.position + new Vector3(
                            -0.5f * size.x,
                            0f,
                            -0.5f * size.z
                        );
        Vector3 max = center + transform.position + new Vector3(
                            0.5f * size.x,
                            0f,
                            0.5f * size.z
                        );
        if (point.x >= min.x && point.x <= max.x)
        {
            if (point.z >= min.z && point.z <= max.z)
            {
                return true;
            }
        }
        return false;
    }
}
