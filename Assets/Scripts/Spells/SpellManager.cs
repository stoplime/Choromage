using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class SpellManager : MonoBehaviour {

    public static string InitializedUnlockedSpellsPath = "/StreamingAssets/bin/initialUnlockedSpells.json";
    public static string UnlockedSpellsPath = "/StreamingAssets/save/UnlockedSpells.json";
    public static string SpellDatabasePath = "/StreamingAssets/bin/SpellDatabase.json";

	private static List<Spell> spellLibrary = new List<Spell>();
    private static List<Spell> unlockedSpellLibrary = new List<Spell>();
    public static List<Spell> SpellLibrary
    {
        get { return spellLibrary; }
    }
	public static List<Spell> UnlockedSpellLibrary
    {
        get { return unlockedSpellLibrary; }
    }

    //private static List<UnlockedSpellTemp> unlockedSpellNamesAndKeys = new List<UnlockedSpellTemp>();
	private static Dictionary<string, int> unlockedSpellNamesAndKeys = new Dictionary<string, int>();
    private static JsonData spellData;
    private static JsonData unlockedSpellData;

    // Use this for initialization
    // void Start () {
		
		
    // }
    void Awake()
    {
        LoadSpells();
    }


    public static void LoadSpells()
    { 
        spellData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + SpellDatabasePath));
		CreateLibrary(spellData);
        if (File.Exists(Application.dataPath + SpellManager.UnlockedSpellsPath))
        {
            unlockedSpellData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + UnlockedSpellsPath));
        }
        else
        {
            unlockedSpellData = null;
        }
		
        ReadUnlockedSpells();
    }

    private static void CreateLibrary(JsonData spellData)
	{
        spellLibrary.Clear();
        for (int i = 0; i < spellData.Count; i++)
		{
			IDictionary dictData = spellData[i] as IDictionary;
            spellLibrary.Add(new Spell(
                                     spellData[i]["Name"].ToString(),
                                     spellData[i]["SpellScriptName"].ToString(),
                                     spellData[i]["SpellElement"].ToString(),
                                     spellData[i]["SpellIcon"].ToString(),
                                     (int)spellData[i]["Mana"],
                                     float.Parse(spellData[i]["CastTime"].ToString()),
                                     float.Parse(spellData[i]["CoolDown"].ToString()) ,
                                     spellData[i]["Prefab"].ToString(),
                                     float.Parse(spellData[i]["Duration"].ToString()),
                                     float.Parse(spellData[i]["MaxDiameter"].ToString()),
                                     float.Parse(spellData[i]["Damage"].ToString())
									 ));

		}
    }

    private static void ReadUnlockedSpells()
    {
        unlockedSpellLibrary.Clear();
        unlockedSpellNamesAndKeys.Clear();
        if (unlockedSpellData != null)
        {
            for (int i = 0; i < unlockedSpellData.Count; i++)
            {
                // IDictionary dictData = unlockedSpellData[i] as IDictionary;
                //TODO: ExtraItemData for hotkeys
                // ExtraItemData eid = null;
                // if (dictData.Contains("ied"))
                // {
                // 	eid = JsonMapper.ToObject<ExtraItemData>(spellData[i]["ied"].ToJson());
                // }
                // unlockedSpellNamesAndKeys.Add(new UnlockedSpellTemp(
                // 						unlockedSpellData[i]["Name"].ToString(),
                //                         (int)unlockedSpellData[i]["HotKey"])
                // 						);
                string key = unlockedSpellData[i]["Name"].ToString();
                if (unlockedSpellNamesAndKeys.ContainsKey(key))
                {
                    unlockedSpellNamesAndKeys.Remove(key);
                }
                unlockedSpellNamesAndKeys.Add(
                                        key,
                                        (int)unlockedSpellData[i]["HotKey"]
                                        );
            }
        }
        CreateUnlockedSpellsLibrary();
    }
    private static void CreateUnlockedSpellsLibrary()
    {
        foreach (Spell spell in spellLibrary)
        {
            if (unlockedSpellNamesAndKeys.ContainsKey(spell.Name))
            {
                spell.Unlock(unlockedSpellNamesAndKeys[spell.Name]);
                unlockedSpellLibrary.Add(spell);
            }
        }
        if (GameManager.Cantis != null)
        { 
            if (GameManager.Cantis.GetComponent<PlayerSpells>() != null)
            {
                GameManager.Cantis.GetComponent<PlayerSpells>().InitializeHotKeys();
            }
        }
    }
    // Update is called once per frame

    public static void UnlockNewSpell(string spellName)
    {
        //TODO: see if better way to find thing in list
        foreach (Spell spell in spellLibrary)
        {
            if (spellName == spell.Name)
            {
                if (!spell.Unlocked)
                {
                    spell.Unlock();
                    unlockedSpellLibrary.Add(spell);
                    break;
                }
            }
        }
    }
    public static Spell FindSpell(string spellName)
    {
        foreach (Spell s in spellLibrary)
        {
            if (s.Name == spellName)
            {
                return s;
            }
        }
        if (spellLibrary.Count == 0)
        {
            print(string.Format("ERROR: Spells not initialized. Could not return {0}.", spellName));
        }
        else
        {
            print(string.Format("ERROR: Spell {0} not found", spellName));
        }
        return null;
    }

    //TODO: Determine private/public
    public static void SaveUnlockedSpells()
    {
        // List<Dictionary<string, string>> currentSpellsUnlocked = new List<Dictionary<string, string>>();
        List<Spell_JSON> currentSpellsUnlocked = new List<Spell_JSON>();
        // foreach (KeyValuePair<string, int> spell in unlockedSpellNamesAndKeys)
        // {
        //     Dictionary<string, string> d = new Dictionary<string, string>();
        //     d.Add(spell.Key, spell.Value.ToString());
        //     currentSpellsUnlocked.Add(d);
        // }
        foreach (Spell spell in unlockedSpellLibrary)
        {
            // Dictionary<string, string> d = new Dictionary<string, string>();
            // d.Add(spell.Name, spell.HotKey.ToString());
            // currentSpellsUnlocked.Add(d);
            currentSpellsUnlocked.Add(spell.ToSpell_JSON());
        }
        JsonData saveUnlocked = JsonMapper.ToJson(currentSpellsUnlocked);
        File.WriteAllText(Application.dataPath + UnlockedSpellsPath, saveUnlocked.ToString());
	}
}
