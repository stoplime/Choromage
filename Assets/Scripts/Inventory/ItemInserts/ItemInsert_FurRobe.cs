using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is designed to insert items to the players inventory
/// durring a dialogue scene based on dialogue variables.
/// </summary>
public class ItemInsert_FurRobe : ItemInserter {
    public ItemInsert_FurRobe()
    {
        VariableFlag = "GiveFurRobe";
        ItemsToInsert.Add(new ItemSaveName("Fur Robe", 1));
    }
}

