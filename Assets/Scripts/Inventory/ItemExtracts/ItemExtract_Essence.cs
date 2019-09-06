using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is designed to Remove a specified item from the players inventory
/// durring a dialogue scene based on dialogue variables.
/// </summary>
public class ItemExtract_Essence : ItemExtractor {
    public ItemExtract_Essence()
    {
        VariableFlag = "RemoveThorinsEssence";
        ItemsToExtract.Add(new ItemSaveName("Cantis Gem", 1));
    }
}

