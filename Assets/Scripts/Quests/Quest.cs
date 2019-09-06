using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quest {
	public class MapTarget{
		public bool Enable;
		public string TargetName;
		/// <summary>
		/// Creates a target
		/// </summary>
		/// <param name="enable">Enables the target in the minimap</param>
		/// <param name="targetName">The target's GameObject name</param>
		public MapTarget(bool enable, string targetName)
		{
			Enable = enable;
			TargetName = targetName;
		}
		public MapTarget()
		{
			Enable = false;
			TargetName = "";
		}
	}

	/// <summary>
	/// A unique ID of the quest. Used in many variables to identify the quest.
	/// </summary>
	public string ID;
	/// <summary>
	/// The title of the quest that the player can see
	/// </summary>
	public string Name;
	/// <summary>
	/// A text description of the quest for the player to see
	/// </summary>
	public string Description;
	/// <summary>
	/// List of requirements in string to show the player in the quest log
	/// </summary>
	/// <typeparam name="string"></typeparam>
	/// <returns></returns>
	public List<string> RequirementDescription = new List<string>();
	/// <summary>
	/// List of goals that indicate the progress of the player in this quest
	/// </summary>
	/// <typeparam name="string"></typeparam>
	/// <typeparam name="bool"></typeparam>
	/// <returns></returns>
	public Dictionary<string, bool> Goals = new Dictionary<string, bool>();
	/// <summary>
	/// The required dialogue variables that needs to be set to complete the quest
	/// </summary>
	/// <typeparam name="string"></typeparam>
	/// <typeparam name="string"></typeparam>
	/// <returns></returns>
	public Dictionary<string, string> DialogueRequirements = new Dictionary<string, string>();
	/// <summary>
	/// The items that indicate that you have achived the quest
	/// </summary>
	/// <typeparam name="ItemSaveName"></typeparam>
	/// <returns></returns>
	public List<ItemSaveName> ItemRequirements = new List<ItemSaveName>();
	/// <summary>
	/// The Minimap Target list
	/// </summary>
	/// <typeparam name="MapTarget"></typeparam>
	/// <returns></returns>
	public List<MapTarget> Targets = new List<MapTarget>();
	/// <summary>
	/// Takes all the items out of the player's inventory that is in the Item Requirements list
	/// </summary>
	public bool TakeItemRequirements = true;
	
	/// <summary>
	/// Accepted indicates that the player has accepted the quest from the dualogue
	/// Quest Dialogue has to have variable "QuestStatus_" + ID which has values: 
	/// 	"accepted_incomplete", "quit_incomplete", "accepted_complete", "goal_incomplete"
	/// "accepted_incomplete": Accepted = true
	/// "quit_incomplete": Accepted = false
	/// </summary>
	public bool Accepted = false;

	/// <summary>
	/// Completed indicates that the player has finished the quest and has turned it in and got your reward
	/// Quest Dialogue has to have variable "QuestStatus_" + ID which has values: 
	/// 	"accepted_incomplete", "quit_incomplete", "accepted_complete", "goal_incomplete"
	/// "accepted_complete": Completed = true
	/// </summary>
	public bool Completed = false;

    /// <summary>
    /// The status is your progress in completing all the tasks on this quest.
    /// This indicates if you can go talk to the quest and get your reward
    /// 	if "QuestStatus_" + ID == "accepted_incomplete" and this is true:
    /// 	"QuestStatus_" + ID = "goal_incomplete"
    /// goal_incomplete indicates that you have finished the goals but not turned it in
    /// </summary>
    /// <value>
    /// True: Fully completed the Goals and can talk to the quest giver
    /// False: Still in progress
    /// </value>
    // public bool TurnedIn = false;

    public bool Status
	{
		get{
			foreach (KeyValuePair<string, bool> goal in Goals)
			{
				if (!goal.Value)
				{
					return false;
				}
			}
			return true;
		}
	}

	public void InitializeGoals()
	{
		// Auto add the DialogueRequirements for QuestStatus
		DialogueRequirements.Add("QuestStatus_" + ID, "accepted_incomplete");


		foreach (KeyValuePair<string, string> dialogueVar in DialogueRequirements)
		{
			Goals.Add("Goal_" + dialogueVar.Key, false);
		}
		for (int i = 0; i < ItemRequirements.Count; i++)
		{
			ItemSaveName item = ItemRequirements[i];
			Goals.Add("Goal_" + item.item, false);
		}
	}

	public List<GameObject> GetTargets()
	{
		List<GameObject> targets = new List<GameObject>();
		foreach (MapTarget mt in Targets)
		{
			if (mt.Enable)
			{
				GameObject target = GameObject.Find(mt.TargetName);
				if (target != null)
				{
					targets.Add(target);
				}
				else
				{
					Help.print("Target {"+mt.TargetName+"} Does not exist");
				}
			}
		}
		return targets;
	}

	public List<string> GetPossibleTargets()
	{
		List<string> targets = new List<string>();
		foreach (MapTarget mt in Targets)
		{
			targets.Add(mt.TargetName);
		}
		return targets;
	}

	public void SetTarget(string targetName, bool value)
	{
		foreach (MapTarget mt in Targets)
		{
			if (mt.TargetName == targetName)
			{
				mt.Enable = value;
				return;
			}
		}
	}
}
