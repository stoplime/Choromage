using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is designed to insert items to the players inventory
/// durring a dialogue scene based on dialogue variables.
/// </summary>
public static class ItemInsertExtractManager
{
    /// <summary>
    /// string = dialogue variable that inserts the item
    /// List ItemSaveName = the items inserted to the player's inventory
    /// </summary>
    /// <returns></returns>
    public static List<ItemInserter> ItemsInsert = new List<ItemInserter>();
    public static List<ItemExtractor> ItemsExtract = new List<ItemExtractor>();

    public static void Initialize() 
	{
        // Inserts
        ItemsInsert.Add(new ItemInsert_ThorinIntroStaff());
        ItemsInsert.Add(new ItemInsert_RoseIntroStaff());
        ItemsInsert.Add(new ItemInsert_ThorinCantis());
        ItemsInsert.Add(new ItemInsert_TavernKeeper());
        ItemsInsert.Add(new ItemInsert_FurRobe());
        ItemsInsert.Add(new ItemInsert_Pants());


        // Extracts
        ItemsExtract.Add(new ItemExtract_Essence());
    }
}

