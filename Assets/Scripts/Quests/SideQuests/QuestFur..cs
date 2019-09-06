using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFur : Quest {
    
    public QuestFur()
    {
        ID = "QuestFur";
        Name = "Fur Quest";
        Description = "Help Cassida Smith aquire some fur from the North in the Hunting Grounds.";
        RequirementDescription.Add("Aquire 5 furs");

        // DialogueRequirements.Add("QuestStatus", "accepted_incomplete");
        ItemRequirements.Add(new ItemSaveName("Fur", 5));
        TakeItemRequirements = true;
        InitializeGoals();
    }
}
