using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is designed to insert items to the players inventory
/// durring a dialogue scene based on dialogue variables.
/// </summary>
public class ItemInsert_RoseIntroStaff : ItemInserter {
    public ItemInsert_RoseIntroStaff()
    {
        VariableFlag = "RoseGivesStaff";
        ItemsToInsert.Add(new ItemSaveName("Uncharged Staff", 1));
    }
}

