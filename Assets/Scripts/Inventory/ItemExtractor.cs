using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is designed to insert items to the players inventory
/// durring a dialogue scene based on dialogue variables.
/// </summary>
public class ItemExtractor
{
    public string VariableFlag;
    public List<ItemSaveName> ItemsToExtract = new List<ItemSaveName>();

    /// <summary>
    /// Inserts the items into the player's inventory
    /// </summary>
    public void ExtractItems()
    {
        for (int i = 0; i < ItemsToExtract.Count; i++)
        {
            for (int j = 0; j < ItemsToExtract[i].count; j++)
            {
                GameManager.playerInventory.RemoveItem(ItemsToExtract[i].item);
            }
        }
    }
}

