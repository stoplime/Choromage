using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIScript : MonoBehaviour {
    float Health
    {
        get
        {
            if (statsScript == null)
            {
                FindStatsScript();
            }
            return statsScript.Health;
        }
    }
    float MaxHealth
    { 
        get
        {
            if (statsScript==null)
            {
                FindStatsScript();
            }
            return statsScript.MaxHealth;
        }
    }
    Slider healthBar;
    Slider manaBar;
    float Mana
    { 
        get
        {
            if (statsScript==null)
            {
                FindStatsScript();
            }
            return statsScript.Mana;
        }
    }
    float MaxMana
    {
        get
        {
            if (statsScript==null)
            {
                FindStatsScript();
            }
            return statsScript.MaxMana;
        }
    }

    //GameObject questHolder;
    bool questButtonPressed = false;
    float experience;
    float nextLvlExp;

    //float gold;
    GameObject statsScreen;
    //Text goldText;
    GameObject deadScreen;

    Vector3 respawnPoint;

    TextMesh healthText;

    PlayerStats statsScript;

    Text healthBarText;
    Text manaBarText;
    Image inventoryFlasher;
    bool flashInventory;
    bool brighter;
    Image journalFlasher;
    bool flashJournal;
    bool brighterJournal;
    float flashSpeed =5f;
    // float alpha;
    string Armor
    {
        get
        {
            if (statsScript == null)
            {
                FindStatsScript();
            }
            string armorString = string.Format("Defense: {0}", statsScript.ArmorDefense);
            if (statsScript.EquipmentSpellStats.TotalResistances != null)
            {
                if (statsScript.EquipmentSpellStats.TotalResistances.ContainsKey(Element.fire))
                { 
                    armorString += string.Format("\nFire Defense: {0}", statsScript.EquipmentSpellStats.TotalResistances[Element.fire]);
                }
            }
            return armorString;
        }

    }


    // Use this for initialization
    //TODO see why gold stat always says 5 beforfe opening stats menu
    void Start () {
        FindUI();
        //healthText = GetComponentInChildren<TextMesh>();
        //StatsText = GameObject.Find("StatsText").GetComponent<Text>();
        respawnPoint = transform.position;
        deadScreen = GameObject.Find("DeadScreenPanel");
        if (deadScreen!= null)
        {
            deadScreen.SetActive(false);
        }
        FindStatsScript();
        StatsDisplay();
    }
    void FindStatsScript()
    {
        statsScript = GetComponent<PlayerStats>();
    }
    /// <summary>
    /// Finds UI objects by name and sets the objects equal to them
    /// </summary>
    void FindUI()
    {
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<Slider>();
        manaBar = GameObject.Find("PlayerManaBar").GetComponent<Slider>();
        healthBarText = GameObject.Find("HealthBarText").GetComponent<Text>();
        manaBarText = GameObject.Find("ManaBarText").GetComponent<Text>();        
        //goldText = GameObject.Find("GoldText").GetComponent<Text>();
        //questHolder = GameObject.Find("PopupQuest");
    }

    // Update is called once per frame
    void Update()
    {
        if (MaxHealth == 0)
        { 
            importStats();
        }
        UpdateCornerStats();
        if (flashInventory)
        {
            FlashInventoryButton();
        }
        if (flashJournal)
        {
            FlashJournalButton();
        }
        //questHolder.SetActive(questButtonPressed);
    }
    public void DeadMenu()
    {
        // deadScreen = GameObject.Find("DeadScreenHolder");
        deadScreen.SetActive(true);
        Time.timeScale = 0;
        // GameManager.DeathSaveGame();

        // foreach (Transform child in deadScreen.transform)
        // {
        //     child.gameObject.SetActive(true);
        // }
    }

     public void Respawn ()
    {
        // GameManager.ReloadScene();
        // LoadManager.instance.LoadPlayerInfo();
        // SceneManager.LoadScene(1);
        // transform.position = respawnPoint;
        // FullHeal();
        // deadScreen = GameObject.Find("DeadScreenHolder");
        // foreach (Transform child in deadScreen.transform)
        // {
        //     child.gameObject.SetActive(false);
        //     GetComponent<PlayerStats>().FullHeal();
        // }
    }
    #region basic ui
    public void UpdateEquippmentStats()
    {
        if (statsScript==null)
        {
            FindStatsScript();
        }
        LeveledUpdate();
    }

    void importStats ()
    {
        if (statsScript==null)
        {
            FindStatsScript();
        }
        experience = statsScript.Experience;
        nextLvlExp = statsScript.NextLevel;
        //gold = statsScript.Gold;
    }

    public void LeveledUpdate()
    {
        if (healthBar == null)
        {
            FindUI();
        }
        // print(maxHealth);
        healthBar.maxValue = MaxHealth;
        manaBar.maxValue = MaxMana;
    }

    
    void UpdateCornerStats ()
    {
        if (healthBar.maxValue == 1)
        {
            LeveledUpdate();
        }
        healthBar.value = Health;
        manaBar.value = Mana;
        
        healthBarText.text = string.Format("{0:##}/{1:##}", Health, MaxHealth);
        if (Mana >= 1)
        {
            manaBarText.text = string.Format("{0:##}/{1:##}", Mana, MaxMana);
        }
        else
        {
            manaBarText.text = string.Format("0/{1:##}", Mana, MaxMana);
        }

        // Help.print(mana, manaBar.value);
        //goldText.text = (gold.ToString() + " gp");
    }

    public void TaskOnClick()
    {
        questButtonPressed = !questButtonPressed;

    }
    public string StatsDisplay()
    {
        string stats = ("Wizard Joey: \n"
            + "\nHealth: " + Health.ToString() + "/" + MaxHealth.ToString()
            + "\nMana: " + Mana.ToString() + "/" + MaxMana.ToString()
            + "\nHealth Regen: " + statsScript.HealthRegenStat.ToString() + " per second" 
            + "\nMana Regen: " + statsScript.ManaRegenStat.ToString() + " per second"
            /*+ "\nGold: " + gold.ToString()*/
            + "\n" + Armor);
        // + "\nCurrent Experience: " + experience.ToString() + "/" + nextLvlExp.ToString());
        // StatsText.text = ("Wizard Joey: \n"
        //     + "\nHealth: " + health.ToString() + "/" + maxHealth.ToString() 
        //     + "\nMana: " + mana.ToString() + "/" + maxMana.ToString()
        //     + "\nGold: " + gold.ToString()
        //     //+ "\nArmor: " + Armor.ToString()
        //     + "\nCurrent Experience: " + experience.ToString() + "/" + nextLvlExp.ToString());
        return stats;
    }
    #endregion

    void SetRespawn()
    {
        respawnPoint = transform.position;
    }

    #region Inventory Flash
    public void InventoryButtonFlash()
    {
        if (inventoryFlasher == null)
        {
            inventoryFlasher = GameObject.Find("NewItemFlash").GetComponent<Image>();
        }
        flashInventory = true;
        brighter = true;
    }
    void FlashInventoryButton()
    {
        float alpha = inventoryFlasher.color.a;
        if (brighter && alpha < .4)
        {
            alpha+= Time.unscaledDeltaTime/flashSpeed;
        }
        else if (brighter)
        {
            brighter = false;
            alpha-= Time.unscaledDeltaTime/flashSpeed;
        }
        else if (alpha >.01)
        {
            alpha-= Time.unscaledDeltaTime/flashSpeed;                
        }
        else
        {
            brighter = true;
            alpha+= Time.unscaledDeltaTime/flashSpeed;
        }
        inventoryFlasher.color = new Color(1, 1, 0, alpha);
    }
    public void StopInventoryFlash()
    { 
         flashInventory = false;
            if (inventoryFlasher != null)
            {
                inventoryFlasher.color = Color.clear;
            }
    }
    #endregion
    #region  journal flash
    public void JournalButtonFlash()
    {
        if (journalFlasher == null)
        {
            journalFlasher = GameObject.Find("NewQuestFlash").GetComponent<Image>();
        }
        flashJournal = true;
        brighterJournal = true;
    }
    void FlashJournalButton()
    {
        float alpha = journalFlasher.color.a;
        if (brighterJournal && alpha < .4)
        {
            alpha+= Time.unscaledDeltaTime/flashSpeed;
        }
        else if (brighterJournal)
        {
            brighterJournal = false;
            alpha-= Time.unscaledDeltaTime/flashSpeed;
        }
        else if (alpha >.01)
        {
            alpha-= Time.unscaledDeltaTime/flashSpeed;                
        }
        else
        {
            brighterJournal = true;
            alpha+= Time.unscaledDeltaTime/flashSpeed;
        }
        journalFlasher.color = new Color(1, 1, 0, alpha);
    }
    public void StopJournalFlash()
    { 
         flashJournal = false;
            if (journalFlasher != null)
            {
                journalFlasher.color = Color.clear;
            }
    }
    #endregion
}
