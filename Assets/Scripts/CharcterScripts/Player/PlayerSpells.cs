using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerSpells : MonoBehaviour
{
    Color unkownColor = new Color(.5f, .5f, .5f, .5f);
    // string emptyIconPath = "UIStuff/Icons/Spells/empty";
    string emptyIconPath = "UI Stuff/Inventory/Items/Crystal Ball_03";
    int maxNumHotKeys = 6;
    //Attack currentSpell = null;
    //Attack castingSpell = null;
    int currentSpellNum = 1;
    int oldSpellNum;

    //private Dictionary<int, Splash> hotSpells = new Dictionary<int, Splash>();
    private Dictionary<int, Spell> hotSpells = new Dictionary<int, Spell>();
    Animator anime;
    //TODO: Get Rid of this
    public Animator Anime
    { get { return anime; } }
    //GameObject cantis;
    private int firstSpellNum = 1;
    public int FirstSpellNum
    {
        get { return firstSpellNum; }
    }
    bool casting;

    public int EmptyHotKeySlot
    {
        get 
        {
            for (int i = firstSpellNum; i <= maxNumHotKeys; i++)
            {
                if (!hotSpells.ContainsKey(i))
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public bool Attacking
    { set { GameManager.player.GetComponent<PlayerController>().PlayerIsAttacking = value; } }
    //bool waitingForMana;
    Slider spellSlider;

    Text[] cooldownTexts = new Text[6];
    //List <Button> hotSpellButtons;
    // Use this for initialization

    SpellCaster spellCasterScript;
    bool HasStaff
    {
        get { return GameManager.player.GetComponent<EquippedStats>().HasStaff||GameManager.FastCasting;}
    }

    public Dictionary<int, Spell> HotSpells
    { get { return hotSpells;} }
    
    
    void Start () {
        GameManager.Cantis = gameObject;
        // InitializeHotKeys();
        anime = GameManager.player.GetComponent<Animator>();

        
        FindCoolDownTexts();
        //hotSpellButtons = new List<Button>();
        spellSlider = GameObject.Find("SpellSelected").GetComponent<Slider>();
        spellCasterScript = GameManager.player.GetComponent<SpellCaster>();
        oldSpellNum = currentSpellNum;
    }
    void Awake()
    {
        InitializeHotKeys();
    }
    public void InitializeHotKeys()
    {
        hotSpells.Clear();   
        foreach (Spell spell in SpellManager.UnlockedSpellLibrary)
        {
            // print(spell.Name);
            if (spell.SpellScript == null)
            {
                if (GameManager.Cantis == null)
                {
                    GameManager.Cantis = gameObject;
                }
                spell.GetScript(GameManager.Cantis);
                
            }
            if (spell.HotKey >= 0)
            {
                int tempHotKey = spell.HotKey;
                if (hotSpells.ContainsKey(tempHotKey))
                {
                    print(string.Format("ERROR: The two spells, {0} and {1} have the same hotkey, {2}", hotSpells[tempHotKey].Name, spell.Name, tempHotKey));
                    hotSpells.Remove(tempHotKey);
                    hotSpells.Add(tempHotKey, spell);
                }
                else 
                { 
                    hotSpells.Add(tempHotKey, spell);
                }
            }
            if (hotSpells.Count == maxNumHotKeys)
            {
                break;
            }
        }

        foreach(KeyValuePair <int, Spell> spell in hotSpells)
        {
            if (spell.Value!=null)
            {
                spell.Value.SetCasterAndFriends(GameManager.player);
            }
        }
        InitializeHotBar();
    }
    void InitializeHotBar()
    {

        for (int i = firstSpellNum; i <= maxNumHotKeys;i++)
        {
            GameObject hotButton = GameObject.Find(string.Format("Spell{0}Button", i));
            if (hotSpells.ContainsKey(i))
            {
                hotButton.GetComponent<Image>().sprite = Resources.Load <Sprite> (hotSpells[i].IconLocation);
                hotButton.GetComponent<Image>().color = Color.white;
            }
            else
            {
                // print("empty");
                hotButton.GetComponent<Image>().sprite = Resources.Load <Sprite> (emptyIconPath);//~~~
                hotButton.GetComponent<Image>().color = unkownColor;
            }
        }
    }
    /// <summary>
    /// For when the player opens up the spell book and changes the hotkeys around
    /// </summary>
    public void SetHotKeys(Dictionary <int,Spell> newHotSpells)
    {
        print("SHK");
        foreach (KeyValuePair <int,Spell> spell in newHotSpells)
        {
            if (spell.Value != hotSpells[spell.Key])
            {
                print(spell.Key);
                ReassignHotKey(spell);
                //TODO: implement changing the icons
            }
        }

        //hotSpells=newHotSpells;
    }

    public void ReassignHotKey(KeyValuePair <int,Spell> spell)
    {
        if (hotSpells.ContainsKey(spell.Key))
        {
            hotSpells[spell.Key].UpdateHotKey();
        }
        else
        {
            hotSpells.Add(spell.Key,spell.Value);
        }
        spell.Value.UpdateHotKey(spell.Key);
        hotSpells.Remove(spell.Key);
        hotSpells.Add(spell.Key, spell.Value);
        UpdateHotSpellImage(spell.Key);
    }
    void UpdateHotSpellImage(int i)
    {
        GameObject spellButton = GameObject.Find(string.Format("Spell{0}Button",i));
        // print(spellButton.name);
        spellButton.GetComponent<Image>().sprite =  Resources.Load <Sprite>(hotSpells[i].IconLocation);
        spellButton.GetComponent<Image>().color = Color.white;
    }


    // Update is called once per frame
    void Update () {
        // foreach (var item in hotSpells)
        // {
        //     Help.print(item.Key, item.Value.Name);
        // }
        if ((!GameManager.player.GetComponent<PlayerController>().Stunned||GameManager.Invulnerable)&&HasStaff)
        {
            // CheckSpellCast(currentSpellNum);
        }
        if (Input.GetKeyDown(KeyCode.U)&& GameManager.GodMode)
        {
            SpellManager.UnlockNewSpell("Iceblast");
            SpellManager.UnlockNewSpell("Earth Spike");
            SpellManager.UnlockNewSpell("Fireball");
            SpellManager.UnlockNewSpell("Gust");
            SpellManager.UnlockNewSpell("HealingBubble");
            SpellManager.UnlockNewSpell("Life Drain");

        }
        CheckHotKeys();
        CooldDownSpells();
        PrintCooldowns();
        HighlightCurrentSpell();
    }

    void HighlightCurrentSpell()
    {
        if (spellSlider != null)
        {
            spellSlider.value = currentSpellNum;
        }
        else
        { 
            spellSlider = GameObject.Find("SpellSelected").GetComponent<Slider>();
        }
    }

    #region Spell Calls
    void CheckHotKeys()
    {
        //if spell hotkey pressed
        if (Input.GetKeyDown(Controls.GetControl("spell1")))
        {
            SpellOne();
        }
        if (Input.GetKeyDown(Controls.GetControl("spell2")))
        {
            SpellTwo();
        }
        if (Input.GetKeyDown(Controls.GetControl("spell3")))
        {
            SpellThree();
        }
        if (Input.GetKeyDown(Controls.GetControl("spell4")))
        {
            SpellFour();
        }
        if (Input.GetKeyDown(Controls.GetControl("spell5")))
        {
            SpellFive();
        }
        if (Input.GetKeyDown(Controls.GetControl("spell6")))
        {
            SpellSix();
        }
        if (Input.GetKeyDown(Controls.GetControl("off_hand")))
        {
            OffHand();
        }
    }

    public void SpellOne()
    {
        SpellSelected(1);
    }
    public void SpellTwo()
    {
        SpellSelected(2);
    }
    public void SpellThree()
    {
        SpellSelected(3);
    }
    public void SpellFour()
    {
        SpellSelected(4);
    }
    public void SpellFive()
    {
        SpellSelected(5);
    }
     public void SpellSix()
    {
        SpellSelected(6);
    }
    public void OffHand()
    {
        // if (!casting)
        // {
            // currentSpellNum = 0;
        //     //currentSpell = GetComponent<Throw>();
        // }
    }

    void SpellSelected(int num)
    {
        if (!casting && hotSpells.ContainsKey(num))
        {
            currentSpellNum = num;
            if (currentSpellNum == oldSpellNum)
            {
                CheckSpellCast(currentSpellNum);
            }
            oldSpellNum = currentSpellNum;
        }
    }
    #endregion

    #region  Cast Spells
    void CheckSpellCast(int spellNum)
    { 
        if (spellNum >=firstSpellNum && hotSpells.ContainsKey(spellNum) &&!casting)
        {
            // print("notnull");
            if (!hotSpells[spellNum].SpellScript.NeedsTarget)
            {
                bool readyToCast = spellCasterScript.CheckCastable(hotSpells[spellNum]);

                if (readyToCast)    //if spell not cooling down and other spell not being cast
                {
                    //if free spells is selected with god mode on, players spells no longer cost mana
                    if (GameManager.FreeSpells)
                    {
                        CastSpell(spellNum);
                    }
                    //if enough mana
                    //needs to be nested inside the readyToCast because if there is enough mana it will decrease inside of CheckMana possibly without casting                
                    else if (spellCasterScript.CheckMana(hotSpells[spellNum]))
                    {
                        CastSpell(spellNum);
                    }
                }
            }
        }
    }

    void CastSpell(int spellNum)
    { 
        casting = true;
        //spellCasterScript.CastSpell(hotSpells[currentSpellNum]);
        hotSpells[spellNum].SpellScript.InitiateAttack(GameManager.player);
        hotSpells[spellNum].ResetTimer();
        anime.SetFloat("castTime", hotSpells[spellNum].CastTime);
        Attacking = true;
        anime.SetTrigger("Attack");
    }
    void CastSpell(int spellNum, RaycastHit target)
    { 
        casting = true;
        //spellCasterScript.CastSpell(hotSpells[currentSpellNum], target);
        hotSpells[currentSpellNum].SpellScript.InitiateAttack(GameManager.player, target.point);
        hotSpells[spellNum].ResetTimer();
        anime.SetFloat("castTime", hotSpells[spellNum].CastTime);
        Attacking = true;
        anime.SetTrigger("Attack");

    }

    void CooldDownSpells()
    {
        foreach (KeyValuePair<int, Spell> spell in hotSpells)
        {
            spell.Value.CoolDownTimer();
        }
    }

    /// <summary>
    /// To test spells outside of spell hotbar
    /// </summary>
    /// <param name="spell"></param>
    public void CastSpell(Spell spell)
    {

        //currentSpellNum = 1;
        casting = true;
        hotSpells[spell.HotKey].SpellScript.InitiateAttack(GameManager.player);
        spell.ResetTimer();
        //spell.InitiateAttack(GameManager.player);
        Attacking = true;
        anime.SetTrigger("Attack");
    }

    /// <summary>
    /// Sets the target position for player's spell and allows the script to stop waiting for input before casting spell
    /// </summary>
    public void OnGUI()
    {
        if ((!GameManager.player.GetComponent<PlayerController>().Stunned||GameManager.Invulnerable)&&HasStaff)
        {
            Event e = Event.current;
            if (e.isMouse)
            {
                if (e.button == 0)
                {
                    int layerMask = (1 << 13 | 1 << 9 | 1 << 15); // 13 = Spell    9 = Environment      15 = EnvNoOverlap
                    // int layerMask = 1 << 13;
                    layerMask = ~layerMask;
                    // int spellLayerMask = 1 << 13;
                    // int layerMaskNPC = 1 << 16;
                    // int lootLayerMask = 1 << 17;
                    // spellLayerMask = ~spellLayerMask;
                    // layerMaskNPC = ~layerMaskNPC;
                    // lootLayerMask = ~lootLayerMask;
                    // print(layerMask);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    //RaycastHit2D hit2D;
                    // Help.print(Physics.Raycast(ray, out hit, 1500, spellLayerMask),currentSpellNum>=firstSpellNum,!EventSystem.current.IsPointerOverGameObject());
                    // int npcLayer = 16;
                    if (Physics.Raycast(ray, out hit, 1500, layerMask) && currentSpellNum >= firstSpellNum && !EventSystem.current.IsPointerOverGameObject())
                    {
                        // if (Physics.Raycast(ray, out hit, 1500, npcLayer))
                        // {
                        //     print("why?");
                        // }
                        if (hit.collider.tag != "NPC"&& hit.collider.tag != "NPC Caster" && hit.collider.tag != "Inventory")
                        {
                            if (hotSpells.ContainsKey(currentSpellNum) && !casting)
                            {
                                bool readyToCast = spellCasterScript.CheckCastable(hotSpells[currentSpellNum]);
                                if (readyToCast && hotSpells[currentSpellNum].SpellScript.NeedsTarget)    //if spell not cooling down and other spell not being cast
                                {
                                    //if free spells is selected with god mode on, players spells no longer cost mana
                                    if (GameManager.FreeSpells)
                                    {
                                        CastSpell(currentSpellNum, hit);
                                    }
                                    //if enough mana
                                    //needs to be nested inside the readyToCast because if there is enough mana it will decrease inside of CheckMana possibly without casting
                                    else if (spellCasterScript.CheckMana(hotSpells[currentSpellNum]))
                                    {
                                        CastSpell(currentSpellNum, hit);
                                    }
                                }
                                else if (readyToCast)
                                { 
                                   //if free spells is selected with god mode on, players spells no longer cost mana
                                    if (GameManager.FreeSpells)
                                    {
                                        CastSpell(currentSpellNum);
                                    }
                                    //if enough mana
                                    //needs to be nested inside the readyToCast because if there is enough mana it will decrease inside of CheckMana possibly without casting                
                                    else if (spellCasterScript.CheckMana(hotSpells[currentSpellNum]))
                                    {
                                        CastSpell(currentSpellNum);
                                    } 
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// The end of the cast spell animation
    /// </summary>
    public void EndAttack()
    {
        hotSpells[currentSpellNum].SpellScript.OnEndAttack();
        // currentSpellNum = 0;
        //currentSpell.OnEndAttack();
        //currentSpell = null;
        casting = false;
        
            // print("done");
    }
    public void SpellInterupted()
    { 
        // currentSpellNum = 0;
        // hotSpells[currentSpellNum].SpellScript.OnEndAttack();
        casting = false;
    }
    // public void AttackingAnimationDone()
    // { 
    //     attacking = false;
    // }
    #endregion

    #region GUI text
    void FindCoolDownTexts()
    {
        for (int i = 0; i < cooldownTexts.Length; i++)
        {
            string findString;
            if (i == 0)
            {
                findString = "Cooldown Text";
            }
            else
            {
                findString = string.Format("Cooldown Text ({0})", i);
               
            }
            cooldownTexts[i] = GameObject.Find(findString).GetComponent<Text>();
        }
    }

    /// <summary>
    /// print cooldown times and whether or not it is being cast for the ui text in the spell hotbar
    /// </summary>
    void PrintCooldowns()
    {
        for(int i = firstSpellNum; i <= cooldownTexts.Length; i++)
        {
            if (i==currentSpellNum &&!casting)
            {
                if (GameManager.player.GetComponent<PlayerController>().Stunned&&(!GameManager.Invulnerable&&!GameManager.Immortal))
                {
                    cooldownTexts[i-1].text = "Cannot cast while unconsious";
                }
                else if (!hotSpells.ContainsKey(i))
                {
                    cooldownTexts[i-1].text = "unknown";
                }
                else if (!HasStaff)
                { 
                    cooldownTexts[i-1].text = "No magic staff equipped.";
                }
                else if (!hotSpells[i].SpellScript.CooledDown && hotSpells[i].SpellScript.NeedsTarget)
                {
                    cooldownTexts[i-1].text = "Cooldown" + hotSpells[i].CoolDownLeft();
                }
                else if (!hotSpells[i].SpellScript.CooledDown)
                {
                    cooldownTexts[i-1].text = hotSpells[i].SpellScript.CoolDownText;
                }
                else if (hotSpells[i].ManaCost > GameManager.player.GetComponent<Stats>().Mana && !GameManager.FreeSpells)
                {
                    cooldownTexts[i-1].text = "Not enough mana.";
                }
                else if (!hotSpells[i].SpellScript.NeedsTarget)
                {
                    cooldownTexts[i-1].text = "Selected";
                }
                else
                {
                    cooldownTexts[i-1].text = "Awaiting Target";
                }
            }
            else if (i==currentSpellNum)
            { 
                cooldownTexts[i-1].text = "Casting";
            }
            else if (hotSpells.ContainsKey(i))
            {
                cooldownTexts[i-1].text = "Cooldown "+ hotSpells[i].SpellScript.CoolDownText;
            }
            else
            {
                cooldownTexts[i-1].text = "";
            }
        }
    }
    #endregion
}
