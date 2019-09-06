using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatEnums
{
	Health,
	MaxHealth,
	HealthRegen,
	Mana,
	MaxMana,
	ManaRegen,
	Gold
}

public class CharacterInfo {
	public int id;
	public string name;
	public int[] position;
	public Dictionary<string, double> stats;
	// Attribute stats

	public Dictionary<string, double> ParseStatsEnum (Dictionary<StatEnums, double> stats)
	{
		Dictionary<string, double> newStats = new Dictionary<string, double>();
		foreach (StatEnums item in Enum.GetValues(typeof(StatEnums)))
		{
			double val;
			stats.TryGetValue(item, out val);
			newStats.Add(Enum.GetName(typeof(StatEnums), item), val);
		}
		return newStats;
	}
	public CharacterInfo(){
		
	}

	public CharacterInfo(int id, string name, Vector2 position, Dictionary<StatEnums, double> stats)
	{
		this.id = id;
		this.name = name;
		this.position = new int[]{(int)position.x, (int)position.y};
		this.stats = ParseStatsEnum(stats);
	}
}
