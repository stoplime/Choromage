using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernKeeperQuest: Quest
{
    public TavernKeeperQuest()
    {
        ID = "TavernKeeperQuest";
        Name = "Help the tavern keeper restock his meat";
        Description = "Hunt wolves for 10 meat and give it to the tavern keeper.";
        RequirementDescription.Add("The tavern is out of meat. He needs 10 to satisfy his customers. Meat comes from wolves");

        ItemRequirements.Add(new ItemSaveName("Meat", 10));
        TakeItemRequirements = true;
        InitializeGoals();
    }
}
