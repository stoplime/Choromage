using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDragonHide : Quest {
    
    public QuestDragonHide()
    {
        ID = "QuestDragonHide";
        Name = "Dragon Hide Quest";
        Description = "Help Cassida Smith aquire some dragons hide from the North in the Hunting Grounds.";
        RequirementDescription.Add("Aquire 5 dragon hides");

        // DialogueRequirements.Add("QuestStatus", "accepted_incomplete");
        ItemRequirements.Add(new ItemSaveName("Dragon Hide", 5));
        TakeItemRequirements = true;
        InitializeGoals();
    }
}
