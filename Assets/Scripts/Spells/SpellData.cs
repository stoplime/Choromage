using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SpellData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Spell spell { get; set; }
    public Spell PopUpSpell{ get { return spell; } }
    private GameObject dragParent;
    private GameObject spellParent;
    private GameObject spellHover;
    private int HotKeyInt
    {
        //TODO Better parse
        get 
        {
            int temp =0;
            switch (spellHover.name)
            { 
                case "Spell1Button":
                    return 1;
                case "Spell2Button":
                    return 2;
                case "Spell3Button":
                    return 3;
                case "Spell4Button":
                    return 4;
                case "Spell5Button":
                    return 5;
            }
            //temp = (int)spellHover.name[5];
            return temp;
        }
    }
    //
    //private List<GameObject> spellHover = new List<GameObject>();
    GameObject clone;
    // Start is called before the first frame update
    void Start()
    {
        dragParent = GameObject.Find ("DraggingParent");

        // // Inventory = GameObject.Find ("Inventory").GetComponent<Inventory> ();
        // stackCountText = GetComponentInChildren<Text> ();
        // stackCountText.text = count.ToString ();
        // hasInitilized = true;
        // UpdateText ();
        // UpdateSlotParent ();
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }
    public void Initialize(Spell parentSpell, GameObject parent)
    {
        spellParent = parent;
        spell = parentSpell;
    }
    public void OnBeginDrag (PointerEventData eventData) {
        //if (Spell != null) {
            //print(gameObject);
            //clone = Instantiate(gameObject, transform.position, Quaternion.identity);
            transform.SetParent (dragParent.transform);
            GetComponent<CanvasGroup> ().blocksRaycasts = false;
        //}
    }
    public void OnDrag (PointerEventData eventData) {
        //if (Item != null) {
            transform.position = eventData.position;
        //}
    }

    public void OnEndDrag (PointerEventData eventData) {
        UpdateSlot ();
        GetComponent<CanvasGroup> ().blocksRaycasts = true;
        // if (spellHover.Count > 0)
        // { 
        //     //
        // }
        //spellHover.Clear();
    }

    public void OnPointerEnter (PointerEventData eventData) {
        //TooltipManager.Activate(Item);
    }

    public void OnPointerExit (PointerEventData eventData) {
        //TooltipManager.Deactivate();
    }
    private void UpdateSlot () {
        // if (!hasInitilized) {
        //     return;
        // }
        if (spellHover!=null)
        {
            print(spellHover.name);
            print(HotKeyInt);
            KeyValuePair<int, Spell> newHotSpell = new KeyValuePair<int, Spell>(HotKeyInt, spell);
            GameManager.Cantis.GetComponent<PlayerSpells>().ReassignHotKey(newHotSpell);
        }
        transform.SetParent (spellParent.transform);
        GetComponent<RectTransform> ().anchoredPosition = spellParent.GetComponent<RectTransform> ().anchoredPosition;
        transform.position = spellParent.transform.position;
    }
    public void OnSpell(GameObject spellButton)
    {
        //spellHover.Add(spellButton);
        spellHover = spellButton;
    }
    public void LeftSpell()
    {
        spellHover = null;
        //spellHover.Remove(spellButton);
    }
}
