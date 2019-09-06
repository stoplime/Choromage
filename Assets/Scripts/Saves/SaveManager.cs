using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public static class SaveManager {
	public static string PlayerDataPath = "/StreamingAssets/save/PlayerInfo.json";
	public static string QuestDataPath = "/StreamingAssets/save/QuestProgress.json";
	public static string DialogueDataPath = "/StreamingAssets/save/DialogueProgress.json";

	// public static CharacterInfo testData = new CharacterInfo(0,"test",new Vector2(0,0),new Dictionary<StatEnums, float>());

	public static Dictionary<string, List<CharacterInfo>> characterDatas = new Dictionary<string, List<CharacterInfo>>();
	
	private static List<JsonData> saveDatas;

	private static void Start() {
		// characterDatas.Add("player", new List<CharacterInfo>());
		// characterDatas["player"].Add(testData);
		// characterDatas.Add("monsters", new List<CharacterInfo>());
		// characterDatas["monsters"].Add(testData);
		// characterDatas["monsters"].Add(testData);

		// SavePlayerInfo();
	}

	private static Dictionary<StatEnums, double> PlayerStatsParser()
	{
		PlayerStats ps = GameManager.player.GetComponent<PlayerStats>();
		Dictionary<StatEnums, double> stats = new Dictionary<StatEnums, double>();
		stats.Add(StatEnums.Health, ps.Health);
		stats.Add(StatEnums.HealthRegen, ps.BaseHealthRegen);
		stats.Add(StatEnums.MaxHealth, ps.BaseMaxHealth);
		stats.Add(StatEnums.Mana, ps.Mana);
		stats.Add(StatEnums.MaxMana, ps.BaseMaxMana);
		stats.Add(StatEnums.ManaRegen, ps.BaseManaRegen);
		stats.Add(StatEnums.Gold, ps.Gold);
		return stats;
	}

	public static void SavePlayerInfo()
	{
		Vector2 playerPos = new Vector2(GameManager.player.transform.position.x, GameManager.player.transform.position.z);
		Dictionary<StatEnums, double> stats = PlayerStatsParser();
		CharacterInfo playerInfo = new CharacterInfo(0, "player", playerPos, stats);
		JsonData playerData = JsonMapper.ToJson(playerInfo);
		File.WriteAllText(Application.dataPath + PlayerDataPath, playerData.ToString());
	}

	public static void SaveQuests()
	{
		JsonData questData = JsonMapper.ToJson(QuestManager.ListOfQuests);
		File.WriteAllText(Application.dataPath + QuestDataPath, questData.ToString());
	}

	public static void SaveDialogue()
	{
		DialogueSaver ds = new DialogueSaver();
		ds.SaveFromDialogBox();
		JsonData dialogueData = JsonMapper.ToJson(ds);
		File.WriteAllText(Application.dataPath + DialogueDataPath, dialogueData.ToString());
	}

	public static void CheckSaveFile()
	{
		string saveFile = Application.dataPath + "/StreamingAssets/save";
		if(!Directory.Exists(saveFile))
		{    
			Directory.CreateDirectory(saveFile);
		}
	}

}
