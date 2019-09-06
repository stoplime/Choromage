using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{

    private List<Spell> allSpells=new List<Spell>();
    private List<Spell> unlockedSpells =new List<Spell>();
    private List<Spell> tempHotSpells;
    private List<GameObject> buttons =new List<GameObject>();
    GameObject spellBookContent;
    Dictionary<Spell, GameObject> spellButtons;
    Dictionary<Spell, GameObject> tempHotSpellButtons;
    private bool spellBookOpen;
    public bool SpellBookOpen 
    { get { return spellBookOpen; } }
    bool reassigningSpell;
    int spellNum;
    Spell selectedSpell;

    // Use this for initialization
    void Start()
    {
        FindContent();
        CreateSpellList();
        //CreateSpellButton();
    }
    /// <summary>
    /// Finds the area where spwlls are viewed
    /// </summary>
    void FindContent()
    {
        if (GameObject.Find("SpellContent") != null)
        {
            spellBookContent = GameObject.Find("SpellContent");
        }
    }
    void CreateSpellList()
    {
        foreach (Spell s in SpellManager.SpellLibrary)
        {
            if (s.Name != "Empty")
            {
                allSpells.Add(s);

                if (s.Unlocked)
                {
                    unlockedSpells.Add(s);
                }
            }
        }
    }
    void CreateSpellButton()
    {
        spellButtons = new Dictionary<Spell, GameObject>();
        foreach (Spell spell in unlockedSpells)
        {
            GameObject btn = Instantiate(Resources.Load("UI Stuff/SpellPrefab")) as GameObject;
            buttons.Add(btn);
            btn.transform.SetParent(spellBookContent.transform);
            btn.GetComponentInChildren<Text>().text = "";
            //btn.GetComponentInChildren<Text>().text = spell.Name;
            btn.GetComponent<Image>().sprite = Resources.Load <Sprite> (spell.IconLocation);
            //create button for each spell
            CreateDraggingChildren(spell,btn);
            spellButtons.Add(spell, btn);
        }
    }
    void CreateDraggingChildren(Spell spell, GameObject spellSlot)
    { 
        GameObject draggingChild = Instantiate(Resources.Load("UI Stuff/SpellDragPrefab")) as GameObject;
        draggingChild.GetComponent<SpellData>().Initialize(spell,spellSlot);
        draggingChild.GetComponent<Image>().sprite = Resources.Load <Sprite> (spell.IconLocation);
        draggingChild.transform.SetParent(spellSlot.transform);
        //draggingChild.GetComponent<RectTransform>() = spellSlot
    }
    void DestroyButtons()
    {
        if (buttons.Count != 0)
        {
            foreach (GameObject button in buttons)
            {
                Destroy(button);
            }
            buttons.Clear();
        }
    }
    public void OpenCloseSpellBook()
    {
        spellBookOpen = !spellBookOpen;
        if (unlockedSpells.Count == 0)
        {
            CreateSpellList();
        }
        FindContent();
        if (spellBookContent.activeInHierarchy)//opening
        { 
            CreateSpellButton();
            tempHotSpells = new List<Spell>();
            tempHotSpellButtons = new Dictionary<Spell, GameObject>();
        }
        else //closing
        {
            print("closing");
            if (tempHotSpells.Count > 0)
            {
                print("ANHB");
                AssignNewHotBar();
            }
            DestroyButtons();
        }

    }
    void AssignNewHotBar()
    {
        Dictionary<int, Spell> newHotBar = new Dictionary<int, Spell>();
        int startingNumber = GameManager.Cantis.GetComponent<PlayerSpells>().FirstSpellNum;
        for (int i = 0; i < tempHotSpells.Count;i++)
        {
            Help.print("new key", i + startingNumber,i,startingNumber);
            newHotBar.Add(i+startingNumber, tempHotSpells[i]);
        }
        GameManager.Cantis.GetComponent<PlayerSpells>().SetHotKeys(newHotBar);
    }
    void InitializeHotBar()
    { 
        
    }
}