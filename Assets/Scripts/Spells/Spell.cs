using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_JSON
{
    public string Name { get; set; }
    public int HotKey { get; set; }
}

public class Spell{

    // Use this for initialization
    //private string name;
    private string name;
    public string Name
    { get { return name; } }

    private Splash spellScript;

    string SpellScriptName { get; set; }
    private Element spellElement;
    public Element SpellElement 
    { get {return spellElement;} }
    List<Element> damagingElements = new List<Element>();
    private bool unlocked;
    public bool Unlocked { get { return unlocked; } }
    public int HotKey { get; set; }
    private string iconLocation;

    private float baseManaCost;
    private float spellDuration;
    private float maxDiameter;
    private float damage;
    private List<GameObject> friends;
    private GameObject caster;
    /// <summary>
    /// # of minimum seconds possible for cast time
    /// </summary>
    float minCastTime = .25f;
    public float ManaCost
    {
        get { return baseManaCost; }
    }
    /// <summary>
    /// how many seconds it takes to cast, because they way the animator works it multiplies, so the inverse must be used, 0 will break it
    /// </summary>
    private float baseCastTime;
    /// <summary>
    /// how many seconds it takes to cast in addition to base cast time, because they way the animator works it multiplies, so the inverse must be used, 0 will break it
    /// </summary>
    private float usersCastTime
    {
        get {return caster.GetComponent<Stats>().CastSpeed; }
    }
    /// <summary>
    /// the multipler for the animator for cast speed, takes the invers of cast times
    /// </summary>
    /// <value></value>
    public float CastTime
    {
        get
        {
            if (baseCastTime != 0&&usersCastTime!=0)
            {
                if ((1 / baseCastTime)+(1/usersCastTime)>0&& (1 / baseCastTime)+(1/usersCastTime)<1/minCastTime)
                {
                    return (1 / baseCastTime) + (1 / usersCastTime);
                }
                else
                {
                    return 1/minCastTime;
                }
            }
            else if (baseCastTime != 0)
            {
                return 1 / baseCastTime;
            }
            else if (usersCastTime!=0)
            {
                return 1+(1/usersCastTime);
            }
            else
            {
                return 1;
            }
        }
    }
    private float spellCoolDown;
    public float CoolDown
    {
        get { return spellCoolDown; }
    }
    public string IconLocation
    {
        get { return iconLocation; }
    }

    public Splash SpellScript 
    { get { return spellScript; } }
    
    //ScriptableObject spellObject;
    string splashObjectPrefab;

    bool cooledDown = true;
    float coolDownTimer;
    public bool ReadyToCast
    { 
        get { return cooledDown;}
    }

    //Splash spellScriptType;

    public Spell(string spellName, string script, string elem, string icon, float mana, float castingTime, float coolDownTime, string prefabName, float duration,float diameter,float dps)
    {
        name = spellName;
        SpellScriptName = script;
        //Unlocked = unlock;
        spellElement = GetElement(elem);
        iconLocation = "UI Stuff/Icons/Spells/" + icon;
        damage = dps;
        baseManaCost = mana;
        baseCastTime = castingTime;
        spellCoolDown = coolDownTime;
        splashObjectPrefab = prefabName;
        spellDuration=duration;
        maxDiameter = diameter;
        SetDamagingElems();
        // if (GameManager.Cantis != null)
        // {
        //     GetScript();
        // }
    }
    public Spell(Spell newSpell)
    {
        name = newSpell.Name;
        SpellScriptName = newSpell.SpellScriptName;
        //Unlocked = unlock;
        spellElement = newSpell.SpellElement;
        iconLocation = newSpell.iconLocation;
        damage = newSpell.damage;
        baseManaCost = newSpell.baseManaCost;
        baseCastTime = newSpell.baseCastTime;
        spellCoolDown = newSpell.spellCoolDown;
        splashObjectPrefab = newSpell.splashObjectPrefab;
        spellDuration= newSpell.spellDuration;
        maxDiameter = newSpell.maxDiameter;
        SetDamagingElems();
    }
    void SetDamagingElems()
    {
        damagingElements.Add(spellElement);
    }
    public void GetScript(GameObject spellCaster)
    {
        spellScript = spellCaster.GetComponent(SpellScriptName) as Splash;
        if (spellScript == null&& spellCaster==GameManager.player)
        { 
            spellScript = GameManager.Cantis.GetComponent(SpellScriptName) as Splash;
        }
        //spellScript.Caster = spellCaster;
        // Help.print(spellScript==null);
        // MonoBehaviour.print(spellScript);
        spellScript.SetSpellComponents(CoolDown, splashObjectPrefab, spellDuration,maxDiameter,damagingElements,damage);
        
    }
    /// <summary>
    /// TODO: Implement
    /// </summary>
    public void SetCasterAndFriends(GameObject spellCaster)
    {
        caster = spellCaster;
        spellScript.SetCaster(caster);
        //spellScript.Caster = caster;
    }
    public void SetCasterAndFriends(GameObject spellCaster, List <GameObject> allies)
    {
        caster = spellCaster;
        friends = allies;
        spellScript.SetCasterAndFriends(caster,friends);
        //spellScript.Caster = caster;
    }

    /// <summary>
    /// Called when reading in spells at the beginning of the game, thus getting the hotkey is needed to figure out the spell hot bar
    /// </summary>
    /// <param name="hKey"></param>
    public void Unlock(int hKey)
    {
        unlocked = true;
        HotKey = hKey;
    }
    /// <summary>
    /// Called when player meets the requirements to unlock a spell, thus the hotkey is not in the hotbar yet so it is set with the default -1
    /// </summary>
    public void Unlock()
    {
        unlocked = true;
        GetScript(GameManager.Cantis);
        SetCasterAndFriends(GameManager.player);
        HotKey = GameManager.Cantis.GetComponent<PlayerSpells>().EmptyHotKeySlot;
        if (HotKey != -1)
        {
            KeyValuePair<int, Spell> newHot = new KeyValuePair<int, Spell>(HotKey, this);
            GameManager.Cantis.GetComponent<PlayerSpells>().ReassignHotKey(newHot);
        }
    }
    public void UpdateHotKey(int hKey)
    {
        HotKey = hKey;
    }
    /// <summary>
    /// For when a spell is removed from the spell hotbar
    /// </summary>
    public void UpdateHotKey()
    {
        HotKey = -1;
    }
    public static Element GetElement(string elem)
    {
        switch (elem.ToLower())
        { 
			case "water":
                return Element.water;
            case "earth":
                return Element.earth;
            case "fire":
                return Element.fire;
            case "air":
                return Element.air;
            case "life":
                return Element.life;
            case "death":
                return Element.death;
        }
        return Element.neutral;
    }
    public void CoolDownTimer()
    {
        if (!cooledDown)
        {
            //print(coolDown);
            coolDownTimer += Time.deltaTime;
			if (coolDownTimer >= CoolDown)
			{
				coolDownTimer = 0;
				cooledDown = true;
			}
        }
    }
    public string ToolTips()
    {
        string tip = string.Format("{0}\nDamage: {1}\nCast time: {2}s\nCooldown: {3}s\nMana cost: {4}", name, damage*spellDuration, CastTime ,CoolDown, ManaCost);
        if (spellElement == Element.death)
        {
            tip += string.Format("\nTurns life energy into mana.");
        }
        return tip;
    }
    public void ResetTimer()
    {
        cooledDown = false;
    }
    public string CoolDownLeft()
    {
        // Help.print(CoolDown, coolDownTimer);
        if (coolDownTimer != 0)
        {
            return string.Format("{0:0.00}",CoolDown - coolDownTimer);
        }
        return "0s";
    }

    public Spell_JSON ToSpell_JSON()
    {
        Spell_JSON sj = new Spell_JSON();
        sj.Name = this.Name;
        sj.HotKey = this.HotKey;
        return sj;
    }
}
