using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IDropHandler {
	public int Index { get; set; }
    public Inventory Inventory { get{ return inventory; } set{ inventory = value; } }

    private Inventory inventory;

    void Start()
	{
	}

	public void OnDrop(PointerEventData eventData)
	{
		// droppedItem is the item being dragged
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
		ItemData thisItemData = null;
		if (transform.childCount > 0)
		{
			thisItemData = transform.GetChild(transform.childCount-1).GetComponent<ItemData>();
		}
		inventory.Swap(droppedItem, Index, thisItemData);
	}
}
