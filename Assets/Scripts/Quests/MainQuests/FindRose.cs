using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRose : Quest
{
    public FindRose()
    {
        ID = "FindRose";
        Name = "Talk to Rose";
        Description = "Thorin told you to go East and follow the path South into town and talk to Rose about getting a better staff.";
        RequirementDescription.Add("Talk to Rose.");
        TakeItemRequirements = false;
        ItemRequirements.Add(new ItemSaveName("Uncharged Staff", 1));

        // Check if you have talked to rose
        // DialogueRequirements.Add("RoseRunicCircleQuest", "accepted_incomplete");
        // Checks if you have the staff the runic circle gives the player
        // ItemRequirements.Add(new ItemSaveName("Novice Staff", 1));

        // Points to the exit of the tutorial forest
        Targets.Add(new MapTarget(false, "QuestForestExitLoc"));
        // Target minimap points to Rose
        Targets.Add(new MapTarget(false, "QuestRoseLoc"));
        // // Targets the Runic Circle
        // Targets.Add(new MapTarget(false, "QuestRunicCircleLoc"));

        InitializeGoals();
    }
}
