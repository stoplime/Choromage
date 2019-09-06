using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseQuest1 : Quest
{
    public RoseQuest1()
    {
        ID = "RoseQuest1";
        Name = "New Magic Staff";
        Description = "Rose asked you to perform your new healing spell at the runic circle located in the easter part of the forest before the river.";
        RequirementDescription.Add("Use your healing spell at the center of the runic circle.");
        TakeItemRequirements = false;
        // Check if you have talked to rose
        // DialogueRequirements.Add("RoseRunicCircleQuest", "accepted_incomplete");
        // Checks if you have the staff the runic circle gives the player
        ItemRequirements.Add(new ItemSaveName("Novice Staff", 1));

        // // Points to the exit of the tutorial forest
        // Targets.Add(new MapTarget(false, "QuestForestExitLoc"));
        // Target minimap points to Rose
        Targets.Add(new MapTarget(false, "QuestRoseLoc"));
        // Targets the Runic Circle
        Targets.Add(new MapTarget(false, "QuestRunicCircleLoc"));

        InitializeGoals();
    }
}
