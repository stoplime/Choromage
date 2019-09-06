using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Item_JSON {
    public int ID { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public int Price { get; set; }
    public int MaxStackSize { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string EquipTag { get; set; }
    public ExtraItemData_JSON EID { get; set; }
}

public class Item {
    public enum EquipmentTag
    {
        Null,
        Cantis,
        Staff,
        Gauntlets,
        OffHand,
        Hat,
        Robe,
        Leggings,
        Boots
    }
    
    public static Item Item_JSON_to_Item(Item_JSON it)
    {
        ExtraItemData new_eid = new ExtraItemData();
        if (it.EID != null)
        {
            new_eid.JSON_To_ExtraItemData(it.EID);
        }
        Item newItem = new Item(
            it.ID,
            it.Name,
            it.Value,
            it.Price,
            it.MaxStackSize,
            it.Description,
            it.Slug,
            it.EquipTag,
            new_eid
        );
        return newItem;
    }

    public static EquipmentTag parseEquipment(string equip)
    {
        switch (equip.ToLower())
        {
            case "cantis":
                return EquipmentTag.Cantis;
            case "staff":
                return EquipmentTag.Staff;
            case "gauntlets":
                return EquipmentTag.Gauntlets;
            case "offhand":
                return EquipmentTag.OffHand;
            case "hat":
                return EquipmentTag.Hat;
            case "robe":
                return EquipmentTag.Robe;
            case "leggings":
                return EquipmentTag.Leggings;
            case "boots":
                return EquipmentTag.Boots;
            default:
                return EquipmentTag.Null;
        }
    }

	public int ID { get; private set;}
	
#region Item Properties
    public string Name { get; set; }
    public int Value { get; set; }
    public int Price { get; set; }
    public int MaxStackSize { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public EquipmentTag EquipTag { get; set; }
    public ExtraItemData EID { get; set; }
#endregion

    public Sprite Sprite { get; set; }

    /// <summary>
    /// Default Constructor is an empty item
    /// </summary>
    public Item()
    {
        this.ID = -1;
    }
    public Item(EquipmentTag et)
    {
        EquipTag = et;
        this.ID = -1;
    }

    /// <summary>
    /// Define the Item, imported from JSON
    /// </summary>
    /// <param name="id">ItemManager.itemLibrary Index</param>
    /// <param name="name">Name of Item</param>
    /// <param name="value">The sell price</param>
    /// <param name="price">The buy price</param>
    /// <param name="maxStack">Maximum number to stack</param>
    /// <param name="consumable">The item can be placed in the off-hand</param>
    /// <param name="description">The tooltip description</param>
    /// <param name="slug">The in-file name of the item to get the Sprite</param>
    /// <param name="equipTag">Which equipment slot it should go into</param>
    public Item(int id, string name, int value, int price, int maxStack, string description, string slug, string equipTag, ExtraItemData eid)
    {
        this.ID = id;
        this.Name = name;
        this.Value = value;
        this.Price = price;
        this.MaxStackSize = maxStack;
        this.Description = description;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("UI Stuff/Inventory/Items/" + slug);
        this.EquipTag = parseEquipment(equipTag);
        this.EID = eid;
    }
    public Item_JSON ToItem_JSON()
    {
        Item_JSON it = new Item_JSON();
        it.ID = this.ID;
        it.Name = this.Name;
        it.Value = this.Value;
        it.Price = this.Price;
        it.MaxStackSize = this.MaxStackSize;
        it.Description = this.Description;
        it.Slug = this.Slug;
        it.EquipTag = this.EquipTag.ToString();
        if (this.EID != null)
        {
            it.EID = this.EID.ToExtraItemData_JSON();
        }

        return it;
    }
}
