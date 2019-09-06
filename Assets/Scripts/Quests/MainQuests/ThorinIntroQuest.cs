using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorinIntroQuest : Quest {
    
    public ThorinIntroQuest()
    {
        ID = "ThorinIntroQuest";
        Name = "Magic Staff Practice";
        Description = "Thorin asked you to test out the magical staff he gave you on a campfire. After you do you should talk to him again.";
        RequirementDescription.Add("Aquire 1 torch");
        TakeItemRequirements = false;

        // Check if you have talked to thorin
        // DialogueRequirements.Add("ThorinIntroQuest", "accepted_incomplete");
        // Checks if you have the staff thorin will give you
        // ItemRequirements.Add(new ItemSaveName("Basic Staff", 1));
        // Checks if you have the torch
        ItemRequirements.Add(new ItemSaveName("Torch", 1));

        InitializeGoals();
    }
}
