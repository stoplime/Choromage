using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is designed to insert items to the players inventory
/// durring a dialogue scene based on dialogue variables.
/// </summary>
public class ItemInserter
{
    public string VariableFlag;
    public List<ItemSaveName> ItemsToInsert = new List<ItemSaveName>();

    /// <summary>
    /// Inserts the items into the player's inventory
    /// </summary>
    public void InsertItems()
    {
        for (int i = 0; i < ItemsToInsert.Count; i++)
        {
            for (int j = 0; j < ItemsToInsert[i].count; j++)
            {
                GameManager.playerInventory.AddItem(ItemsToInsert[i].item);
            }
            if (ItemsToInsert[i].item != "Torch")
            {
                GameManager.player.GetComponent<PlayerUIScript>().InventoryButtonFlash();
            }
        }
    }
}

