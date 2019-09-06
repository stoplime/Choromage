using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ItemSaveName
{
	public string item;
	public int count;
	
	public ItemSaveName(string itemName, int count)
	{
		this.item = itemName;
		this.count = count;
	}
}

[Serializable]
public class ItemSave
{
	public Item_JSON item;
	public int count;

	public ItemSave()
	{

	}

	public ItemSave(Item_JSON item, int count)
	{
		this.item = item;
		this.count = count;
	}
}

public class Inventory : MonoBehaviour {
	public enum InventoryType
	{
		PlayerInventory,
		PlayerEquipment,
		OtherInventory,
		TrashInventory
	}
	
	private int inventoryLayer = 10;

	// Static references
	private static GameObject slotPrefab = null;
	private static GameObject itemPrefab = null;
	private static GameObject inventoryPanel = null;
	private static GameObject itemPanel = null;
	private static GameObject otherItemPanel = null;
	private static GameObject equipmentPanel = null;

	// Unity editor config
	public int slotCount = 25;
	public InventoryType Type = InventoryType.OtherInventory;
	public List<ItemSaveName> InitialItems;

    public Item GetItem(int index)
    {
        return items[index];
    }
	public bool SetItem(int index, Item item)
	{
		// Checks if the equipment slot matches
		if (Type == InventoryType.PlayerEquipment && EquipmentFilter[index] != Item.EquipmentTag.Null && item.EquipTag != EquipmentFilter[index] && item.ID != -1)
			return false;
		items[index] = item;
		return true;
	}
    private List<Item> items = new List<Item>();
	
	public List<GameObject> Slots { get{ return slots; } }
	private List<GameObject> slots = new List<GameObject>();

	private List<Item.EquipmentTag> EquipmentFilter = new List<Item.EquipmentTag>();

	public bool IsEmpty
	{
		get{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].ID != -1)
				{
					return false;
				}
			}
			return true;
		}
	}

    public bool HasInitialized
    {
        get
        {
            return hasInitialized;
        }
    }

    private bool hasInitialized = false;
	private bool beingLoaded = false;

	private static bool once = false;

    private void Awake()
	{
		// initializes the static references
		if (itemPrefab == null) {
			itemPrefab = Resources.Load<GameObject>("UI Stuff/Inventory/ItemImage");
		}
		if (slotPrefab == null) {
			slotPrefab = Resources.Load<GameObject>("UI Stuff/Inventory/ItemSlotPrefab");
		}
		if (inventoryPanel == null) {
			inventoryPanel = GameObject.Find("InventoryPanel");
		}
		if (itemPanel == null) {
			itemPanel = inventoryPanel.transform.Find("ItemPanel").gameObject;
		}
		if (otherItemPanel == null) {
			otherItemPanel = inventoryPanel.transform.Find("OtherInventoryPanel").gameObject;
		}
		if (equipmentPanel == null) {
			equipmentPanel = inventoryPanel.transform.Find("EquiptmentPanel").gameObject;
		}
	}

	private void InitializeItemsSlots()
	{
		ItemSlot itemSlotObj;
		switch (Type)
		{
			case InventoryType.PlayerEquipment:
				// Takes the slots from the equipmentPanel and assigns them into the inventory
				for (int i = 0; i < equipmentPanel.transform.childCount; i++)
				{
					items.Add(new Item());
					slots.Add(equipmentPanel.transform.GetChild(i).gameObject);
					itemSlotObj = slots[i].GetComponent<ItemSlot>();
					itemSlotObj.Inventory = this;
					itemSlotObj.Index = i;
					EquipmentFilter.Add(Item.parseEquipment(slots[i].name));
				}
				break;
			case InventoryType.PlayerInventory:
				// Initializes the slots in the regular itemPanel
				for (int i = 0; i < slotCount; i++)
				{
					items.Add(new Item());
					slots.Add(Instantiate<GameObject>(slotPrefab, itemPanel.transform));
					itemSlotObj = slots[i].GetComponent<ItemSlot>();
					itemSlotObj.Inventory = this;
					itemSlotObj.Index = i;
				}
				break;
			case InventoryType.OtherInventory:
				// Initializes the slots in the otherItemPanel
				for (int i = 0; i < slotCount; i++)
				{
					items.Add(new Item());
					slots.Add(Instantiate<GameObject>(slotPrefab, otherItemPanel.transform));
					itemSlotObj = slots[i].GetComponent<ItemSlot>();
					itemSlotObj.Inventory = this;
					itemSlotObj.Index = i;
				}
				break;
			case InventoryType.TrashInventory:
				items.Add(new Item());
				slots.Add(transform.GetChild(0).gameObject);
				itemSlotObj = slots[0].GetComponent<ItemSlot>();
				itemSlotObj.Inventory = this;
				itemSlotObj.Index = 0;
				break;
			default:
				break;
		}
		
		if (!beingLoaded)
		{
			for (int i = 0; i < InitialItems.Count; i++)
			{
				for (int j = 0; j < InitialItems[i].count; j++)
				{
					AddItem(InitialItems[i].item);
				}
			}
		}
	}

	void Start()
	{
		if (hasInitialized)
			return;
		
		InitializeItemsSlots();

		OnDisableInventory();

		hasInitialized = true;
		if (!once)
		{
			once = true;
			inventoryPanel.SetActive(false);
		}
	}

	void Update()
	{
		// TODO: *************** DEBUG ONLY * Please Remove On Build ***********************
		// if (Input.GetKeyDown(KeyCode.I))
		// {
		// 	for (int i = 0; i < ItemManager.GetItemLibCount(); i++)
		// 	{
		// 		AddItem(i);
		// 	}
		// }
		// *********************************************************************************

		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << inventoryLayer;

		// OnMouseDown
		if (Input.GetMouseButtonDown(0))
		{
			if (Physics.Raycast(ray, out hit, 300f, layerMask))
			{
				if (hit.transform.gameObject == gameObject && Type == InventoryType.OtherInventory && !TopMenu.isOpen)
				{
					InventoryPanelManager.CurrentOtherInventory = this;
					TopMenu topMenuScript = GameObject.Find("TopMenu").GetComponent<TopMenu>();
					topMenuScript.Inventory(true);
					OnEnableInventory();
				}
			}
		}
	}

	// /// <summary>
	// /// Opens other inventories such as loot bags by clicking on it
	// /// </summary>
	// private void OnMouseDown()
	// {
	// 	if (Type == InventoryType.OtherInventory && !TopMenu.isOpen)
	// 	{
	// 		print("Loot bag clicked");
	// 		InventoryPanelManager.CurrentOtherInventory = this;
	// 		TopMenu topMenuScript = GameObject.Find("TopMenu").GetComponent<TopMenu>();
	// 		topMenuScript.Inventory(true);
	// 		OnEnableInventory();
	// 	}
	// }

	/// <summary>
	/// When the other inventory is disabled, it needs to disable its corresponding slots
	/// </summary>
	public void OnDisableInventory()
	{
		if (Type == InventoryType.OtherInventory)
		{
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].SetActive(false);
			}
		}
	}

	/// <summary>
	/// When the other inventory is enabled, it needs to enable its corresponding slots
	/// </summary>
	public void OnEnableInventory()
	{
		if (Type == InventoryType.OtherInventory)
		{
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].SetActive(true);
			}
		}
	}

	/// <summary>
	/// When the other inventory is destoryed, it needs to destroy all its corresponding slots
	/// </summary>
	private void OnDestroy()
	{
		if (Type == InventoryType.OtherInventory)
		{
			for (int i = slots.Count-1; i >= 0; i--)
			{
				Destroy(slots[i]);
			}
		}
	}

	public void CheckQuests()
	{
		if (Type != InventoryType.PlayerInventory)
		{
			return;
		}
		for (int i = 0; i < QuestManager.ListOfQuests.Count; i++)
        {
            Quest thisQuest = QuestManager.ListOfQuests[i];
            for (int j = 0; j < thisQuest.ItemRequirements.Count; j++)
			{
				ItemSaveName questItem = thisQuest.ItemRequirements[j];
				if (FindItem(questItem.item) >= questItem.count)
				{
					thisQuest.Goals["Goal_" + questItem.item] = true;
                    // Help.print("Checks quest items", questItem.item, FindItem(questItem.item));
				}
			}
			// Checks if the player has completed the status and updates the QuestStatus
			if (thisQuest.Status && thisQuest.Accepted)
			{
				GameManager.DialogueBox.Variables["QuestStatus_" + thisQuest.ID] = "goal_incomplete";
				// Help.print("QuestStatus_" + thisQuest.ID, GameManager.DialogueBox.Variables["QuestStatus_" + thisQuest.ID]);
			}
        }
	}

	public bool RemoveItem(int id)
	{
		Item itemToRemove = ItemManager.GetItemLib(id);
		return RemoveItem(itemToRemove);
	}

	public bool RemoveItem(string name)
	{
		Item itemToRemove = ItemManager.GetItemLibByTitle(name);
		if (itemToRemove == new Item())
		{
			return false;
		}
		return RemoveItem(itemToRemove);
	}

	public bool RemoveItem(Item itemToRemove)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].ID == itemToRemove.ID)
			{
				ItemData itemInInventory = slots[i].GetComponentInChildren<ItemData>();
				if(itemInInventory==null)
				{
					return false;
				}
				if (itemInInventory.Count > 1)
				{
					itemInInventory.Count--;
					return true;
				}
				else if (itemInInventory.Count == 1)
				{
					SetItem(i, new Item());
					Destroy(itemInInventory.gameObject);
					return true;
				}
			}
		}
		return false;
	}

	public int FindItem(int id)
	{
		Item itemToFind = ItemManager.GetItemLib(id);
		return FindItem(itemToFind);
	}

	public int FindItem(string name)
	{
		Item itemToFind = ItemManager.GetItemLibByTitle(name);
		if (itemToFind == new Item())
		{
			return 0;
		}
		return FindItem(itemToFind);
	}

	public int FindItem(Item itemToFind)
	{
		int count = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].ID == itemToFind.ID)
			{
				ItemData itemInInventory = slots[i].GetComponentInChildren<ItemData>();
				// print(slots[i]);
				// print(slots[i].GetComponentInChildren<ItemData>());
				if (itemInInventory != null)
				{
					// print(itemInInventory.Count);
					count += itemInInventory.Count;
				}
			}
		}
		return count;
	}

	/// <summary>
	/// Creates an Item in this inventory
	/// </summary>
	/// <param name="id">The id of the item</param>
	/// <returns></returns>
	public bool AddItem(int id)
	{
		Item itemToAdd = ItemManager.GetItemLib(id);
		return AddItem(itemToAdd);
	}
	/// <summary>
	/// Creates an Item in this inventory
	/// </summary>
	/// <param name="name">The name of the item</param>
	/// <returns></returns>
	public bool AddItem(string name)
	{
		Item itemToAdd = ItemManager.GetItemLibByTitle(name);
		if (itemToAdd == new Item())
		{
			return false;
		}
		return AddItem(itemToAdd);
	}

	/// <summary>
	/// Creates an Item in this inventory
	/// </summary>
	/// <param name="itemToAdd">The item</param>
	/// <returns></returns>
	public bool AddItem(Item itemToAdd)
	{
		// check if there already exits an item
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].ID == itemToAdd.ID)
			{
				ItemData itemInInventory = slots[i].GetComponentInChildren<ItemData>();
                // print(itemToAdd.Name);
                if (itemInInventory != null && itemInInventory.Count < itemToAdd.MaxStackSize)
				{
					itemInInventory.Count++;
					CheckQuests();
					return true;
				}
			}
		}

		// create new item with a stack count of 1
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].ID == -1)
			{
				if(SetItem(i, itemToAdd))
				{
					GameObject itemObj = Instantiate<GameObject>(itemPrefab, slots[i].transform);
					itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
					ItemData itemObjData = itemObj.GetComponent<ItemData>();
					itemObjData.Inventory = this;
					itemObjData.Count = 1;
					itemObjData.Item = itemToAdd;
					itemObjData.SlotIndex = i;
					CheckEquipment(itemToAdd);
					CheckQuests();
					return true;
				}
			}
		}

		return false;
	}
	public void CheckEquipment(Item addedItem, Item removedItem)
	{
		if (Type != InventoryType.PlayerEquipment)
			return;
		
		EquippedStats es = GameManager.player.GetComponent<EquippedStats>();
		es.Unequip(removedItem.EquipTag);
		es.Equip(addedItem.EquipTag, addedItem.EID);
        // add more logic to actually add all the item uses
    }
	public void CheckEquipment(Item addedItem)
	{
		if (Type != InventoryType.PlayerEquipment)
			return;
		
		EquippedStats es = GameManager.player.GetComponent<EquippedStats>();
        if (addedItem.ID == -1)
        { 
			es.Unequip(addedItem.EquipTag);
		}
        else 
		{ 
			es.Equip(addedItem.EquipTag, addedItem.EID);
		}
        // add more logic to actually add all the item uses
    }

	public bool Swap(ItemData droppedItem, int slotIndex, ItemData thisItemData=null){
		// droppedItem is the item from the mouse
		// This is called by the inventory that the droppedItem falls on
		bool successful = true;
		// check if the slot is empty
		if (GetItem(slotIndex).ID == -1)
		{
			successful = successful && droppedItem.Inventory.SetItem(droppedItem.SlotIndex, new Item());
			successful = successful && SetItem(slotIndex, droppedItem.Item);
			
			if (successful)
			{
				droppedItem.Inventory.CheckEquipment(new Item(droppedItem.Item.EquipTag));
				droppedItem.Inventory = this;
				droppedItem.SlotIndex = slotIndex;
				CheckEquipment(droppedItem.Item);
				//droppedItem.Inventory.CheckEquipment(new Item());
			}
		}
		// not dropped into the original slot
		else if (droppedItem.SlotIndex != slotIndex || droppedItem.Inventory != this)
		{
			// thisItemData is the item that was on this slot
			if (thisItemData == null)
			{
				return false;
			}
			// check if its the trash
			if (Type == InventoryType.TrashInventory)
			{
				// swap only the Item from the dropped to the trash, not the other way around
				successful = successful && droppedItem.Inventory.SetItem(droppedItem.SlotIndex, new Item());
				successful = successful && SetItem(slotIndex, droppedItem.Item);
				
				if (successful)
				{
					droppedItem.Inventory = this;
					droppedItem.SlotIndex = slotIndex;
					Destroy(thisItemData.gameObject);
				}
				return successful;
			}

			// Check to add to stack
			if (droppedItem.Item.ID == thisItemData.Item.ID)
			{
				// Check if the stack overflows
				int totalStack = droppedItem.Count + thisItemData.Count;
				if (totalStack > thisItemData.Item.MaxStackSize)
				{
					droppedItem.Count = totalStack - thisItemData.Item.MaxStackSize;
					// print(droppedItem.Count);
					thisItemData.Count = thisItemData.Item.MaxStackSize;
					// no update the dropped item slotindex so that it snaps back to the original slot
				}
				else
				{
					// Stack does not over flow
					successful = successful && droppedItem.Inventory.SetItem(droppedItem.SlotIndex, new Item());
					if (successful)
					{
						thisItemData.Count = totalStack;
						Destroy(droppedItem.gameObject);
					}
				}
			}
			else
			{
				// Swap items
				successful = successful && droppedItem.Inventory.SetItem(droppedItem.SlotIndex, thisItemData.Item);
				successful = successful && SetItem(slotIndex, droppedItem.Item);

				if (successful)
				{
					thisItemData.Inventory = droppedItem.Inventory;
					droppedItem.Inventory = this;

					thisItemData.SlotIndex = droppedItem.SlotIndex;
					droppedItem.SlotIndex = slotIndex;
					
					CheckEquipment(droppedItem.Item, thisItemData.Item);
					thisItemData.Inventory.CheckEquipment(thisItemData.Item, droppedItem.Item);
				}
			}
		}
        CheckQuests();
		return successful;
	}

	private void InstantiateItems()
	{
		switch (Type)
		{
			case InventoryType.PlayerEquipment:
				// Takes the slots from the equipmentPanel and assigns them into the inventory
				for (int i = 0; i < equipmentPanel.transform.childCount; i++)
				{
					items.Add(new Item());
				}
				break;
			case InventoryType.PlayerInventory:
				// Initializes the slots in the regular itemPanel
				for (int i = 0; i < slotCount; i++)
				{
					items.Add(new Item());
				}
				break;
			case InventoryType.OtherInventory:
				// Initializes the slots in the otherItemPanel
				for (int i = 0; i < slotCount; i++)
				{
					items.Add(new Item());
				}
				break;
			case InventoryType.TrashInventory:
				items.Add(new Item());
				break;
			default:
				break;
		}
	}

	public List<ItemSave> Save()
	{
		List<ItemSave> saveData = new List<ItemSave>();
		for (int i = 0; i < items.Count; i++)
		{
			if (slots.Count <= i || slots[i] == null || items[i] == null || slots[i].GetComponentInChildren<ItemData>() == null)
				continue;
			ItemSave it = new ItemSave(items[i].ToItem_JSON(), slots[i].GetComponentInChildren<ItemData>().Count);
			// Help.print("ItemSave", it);
			saveData.Add(it);
		}
		return saveData;
	}

	public void Load(List<ItemSave> saveData)
	{
		beingLoaded = true;
		if (!hasInitialized)
		{
			Start();
		}
		for (int i = 0; i < saveData.Count; i++)
		{
			for (int j = 0; j < saveData[i].count; j++)
			{
				AddItem(Item.Item_JSON_to_Item(saveData[i].item));
			}
		}
		beingLoaded = false;
	}
}
