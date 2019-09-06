using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorinNecromancyQuest : Quest
{
    public ThorinNecromancyQuest()
    {
        ID = "ThorinNecromancyQuest";
        Name = "Help Thorin with his Necromancy Magic";
        Description = "Acquire the dragon's essense and give it to Thorin.";
        RequirementDescription.Add("Go to the cave and defeat the dragon with the Cantis and then give it to Thorin before Rose finds out.");

        // Thorin gives you a Cantis to extract the dragon essense
        // ItemRequirements.Add(new ItemSaveName("Cantis", 1));
        // The cantis charges automatically as the player defeats the dragon
        ItemRequirements.Add(new ItemSaveName("Charged Cantis", 1));

        // Target minimap points to Thorin's house
        Targets.Add(new MapTarget(false, "QuestThorinsHouseLoc"));
        // Targets the Cave
        // Targets.Add(new MapTarget(false, "QuestCaveLoc"));
        // Targets back to Thorin's house

        InitializeGoals();
    }
}
