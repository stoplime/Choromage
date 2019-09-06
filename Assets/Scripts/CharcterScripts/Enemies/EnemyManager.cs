using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public static class EnemyManager {
	public static List<GameObject> Enemies = new List<GameObject>();
    public static Dictionary<string, BossEnemy> BossEnemies;
    public static string SlainEnemiesPath = "/StreamingAssets/save/BossesKilled.json";
    private static JsonData slainEnemies;
    private static List<string> slainEnemyNames;
    public static void Initialize()
    {
        Bosses();
        Enemies.Add(Resources.Load("Characters/Enemies/Goblin1") as GameObject);
        Enemies.Add(Resources.Load("Characters/Enemies/MyShiny") as GameObject);
        Enemies.Add(Resources.Load("Characters/Enemies/wolf") as GameObject);

        // BossEnemies.Add("Mother Dragon",Resources.Load("Characters/Enemies/WarFireDragon") as GameObject);
    }
    static void Bosses()
    {
        BossEnemies= new Dictionary<string, BossEnemy>();
        BossEnemy Dragon = new BossEnemy(1215.858f, -2032.508f, Resources.Load("Characters/Enemies/Bosses/WarFireDragon") as GameObject);
        BossEnemy Wolf = new BossEnemy(1583.2f, -2040.4f, Resources.Load("Characters/Enemies/Bosses/TutorialWolf") as GameObject);
        
        BossEnemies.Add("Mother Dragon", Dragon);
        BossEnemies.Add("Tutorial Wolf", Wolf);
        
    }
    static void ReadInSlain()
    {
        slainEnemies = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + SlainEnemiesPath));
        slainEnemyNames = new List<string>();
        for (int i = 0; i < slainEnemies.Count; i++)
		{
			slainEnemyNames.Add(slainEnemies[i]["Name"].ToString());
		}
    }
    public static void InstantiateBosses(string boss)
    {
        if (BossEnemies.ContainsKey(boss)&&!CheckSlain(boss))
        {
            GameObject renameMe = MonoBehaviour.Instantiate(BossEnemies[boss].Prefab, BossEnemies[boss].SpawnPos, Quaternion.identity, GameObject.Find("Bosses").transform);
            renameMe.name = boss;
            // MonoBehaviour.print(string.Format("Instantiated {0}", boss));
        }
        else if(BossEnemies.ContainsKey(boss))
        {
            // MonoBehaviour.print(string.Format("{0} already slain", boss));
        }
        else
        {
            // MonoBehaviour.print(string.Format("Could not instantiate {0}", boss));
        }
    }
    static bool CheckSlain(string bosName)
    {
        if (slainEnemyNames==null)
        {
            return false;
        }
        if (slainEnemyNames.Count==0)
        {
            return false;
        }
        if(slainEnemyNames.Contains(bosName))
        {
            return true;
        }
        return false;
    }
    public static void BossKilled(string bosName)
    {
        // BossEnemies[bosName].Slain = true;
        BossEnemies[bosName].GetSlain();
    }
    static void SaveSlainEnemies()
    { 
        foreach (KeyValuePair<string, BossEnemy> boss in BossEnemies)
        {
            if (boss.Value.Slain)
            { 
                //TODO: implement saving of slain boss names
                // JsonData killed 
            }
        }
    }
}
