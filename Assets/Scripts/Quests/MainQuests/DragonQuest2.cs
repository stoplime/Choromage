using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonQuest2 : Quest
{
    public DragonQuest2()
    {
        ID = "DragonQuest2";
        Name = "Defeat the Dragon";
        Description = "Defeat the dragon at Northern cave.";
        RequirementDescription.Add("Go to the cave and defeat the dragon.");
        TakeItemRequirements = false;

        // Item Drop from the dragon
        ItemRequirements.Add(new ItemSaveName("Large Dragon Hide", 1));

        // Target minimap points to the bridge
        Targets.Add(new MapTarget(false, "QuestBridgeLoc"));
        // Targets the Cave
        Targets.Add(new MapTarget(false, "QuestCaveLoc"));
        // Targets back to Rose
        Targets.Add(new MapTarget(false, "QuestRoseLoc"));

        InitializeGoals();
    }
}
