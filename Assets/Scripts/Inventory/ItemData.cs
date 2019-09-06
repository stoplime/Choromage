using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
    /// <summary>
    /// Reference Item
    /// </summary>
    /// <value></value>
    public Item Item { get; set; }

    /// <summary>
    /// Inventory Index
    /// </summary>
    /// <value></value>
    public int SlotIndex {
        get {
            return slotIndex;
        }
        set {
            slotIndex = value;
            UpdateSlotParent ();
        }
    }

    /// <summary>
    /// Current Stack count
    /// </summary>
    /// <value></value>
    public int Count {
        get {
            return count;
        }
        set {
            count = value;
            UpdateText ();
        }
    }
    
    /// <summary>
    /// The referenced Inventory this item belongs to
    /// </summary>
    /// <value></value>
    public Inventory Inventory {
        get {
            return inventory;
        }
        set {
            inventory = value;
        }
    }

    private Text stackCountText;
    private int count;
    private Inventory inventory;
    private bool hasInitilized = false;
    private int slotIndex;
    private GameObject dragParent;

    void Start () {
        dragParent = GameObject.Find ("DraggingParent");
        // Inventory = GameObject.Find ("Inventory").GetComponent<Inventory> ();
        stackCountText = GetComponentInChildren<Text> ();
        stackCountText.text = count.ToString ();
        hasInitilized = true;
        UpdateText ();
        UpdateSlotParent ();
    }

    /// <summary>
    /// Updates the Text to display the count when there are 2 or more items
    /// </summary>
    private void UpdateText () {
        if (!hasInitilized) {
            return;
        }
        // Checks if the Count is 1 or less, then the text is removed
        if (count <= 1) {
            if (stackCountText.text != "") {
                stackCountText.text = "";
                // print(stackCountText.text);
                return;
            }
        } else {
            int textCount;
            int.TryParse (stackCountText.text, out textCount);
            if (textCount != count) {
                stackCountText.text = count.ToString ();
                // print(stackCountText.text);
            }
        }
    }

    /// <summary>
    /// Whenever the SlotIndex is updated, it will automatically change its transform parent to the correct slot
    /// </summary>
    private void UpdateSlotParent () {
        if (!hasInitilized) {
            return;
        }
        transform.SetParent (Inventory.Slots[slotIndex].transform);
        GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
    }

    public void OnBeginDrag (PointerEventData eventData) {
        if (Item != null) {
            // print(gameObject);
            transform.SetParent (dragParent.transform);
            GetComponent<CanvasGroup> ().blocksRaycasts = false;
        }
    }

    public void OnDrag (PointerEventData eventData) {
        if (Item != null) {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag (PointerEventData eventData) {
        UpdateSlotParent ();
        GetComponent<CanvasGroup> ().blocksRaycasts = true;
    }

    public void OnPointerEnter (PointerEventData eventData) {
        TooltipManager.Activate(Item);
    }

    public void OnPointerExit (PointerEventData eventData) {
        TooltipManager.Deactivate();
    }
}