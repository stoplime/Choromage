using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is designed to insert items to the players inventory
/// durring a dialogue scene based on dialogue variables.
/// </summary>
public class ItemInsert_ThorinIntroStaff : ItemInserter {
    public ItemInsert_ThorinIntroStaff()
    {
        VariableFlag = "ThorinGiveStaff";
        ItemsToInsert.Add(new ItemSaveName("Basic Staff", 1));
    }
}

