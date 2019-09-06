using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour {

    public static bool isOpen
    {
        get
        {
            return (currentOpenWindow != WindowNames.none);
        }
    }

    /// <summary>
    /// List of Windows as an enum
    /// </summary>
    private enum WindowNames
    {
        none,
        inventory,
        spellBook,
        stats,
        journal,
        settings,
        debug,
        help,
        extraHelp
    }

    /// <summary>
    /// A collection of all the Windows
    /// </summary>
    /// <typeparam name="WindowNames"></typeparam>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    private Dictionary<WindowNames, GameObject> windows = new Dictionary<WindowNames, GameObject>();


    private static WindowNames currentOpenWindow = WindowNames.none;

    Text statsText;
    Text currentQuests;
    Text completedQuests;
    Button questTypeChangeButton;
    Button turnHelpPanelOn;
    Button helpPanelToOptions;
    Button helpPanelToExtraHelp;
    Button extraHelpPanelToHelp;
    bool questTypeCurrent;
    AudioSource openerSound;

    // Use this for initialization
    void Start () {
		windows.Add(WindowNames.inventory,  GameObject.Find("InventoryHolder"));
		windows.Add(WindowNames.spellBook,  GameObject.Find("SpellBookHolder"));
		windows.Add(WindowNames.stats,      GameObject.Find("StatsHolder"));
		windows.Add(WindowNames.journal,    GameObject.Find("JournalHolder"));
		windows.Add(WindowNames.settings,   GameObject.Find("SettingsHolder"));
        windows.Add(WindowNames.debug,      GameObject.Find("DebugSettingsHolder"));
        windows.Add(WindowNames.help,       GameObject.Find("HelpMenuHolder"));
        windows.Add(WindowNames.extraHelp,  GameObject.Find("ExtraHelpMenuHolder"));
        questTypeCurrent = true;
        openerSound = GetComponent<AudioSource>();
        CloseAll();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(Controls.GetControl("inventory")))
        {
            Inventory();
        }
        if (Input.GetKeyDown(Controls.GetControl("spellBook")))
        {
            SpellBook();
        }
        if (Input.GetKeyDown(Controls.GetControl("stats")))
        {
            Stats();
        }
        if (Input.GetKeyDown(Controls.GetControl("journal")))
        {
            Journal();
        }
        if (Input.GetKeyDown(Controls.GetControl("settings")))
        {
            Settings();
        }
        if (Input.GetKeyDown(Controls.GetControl("debug"))&&GameManager.GodMode)
        {
            DebugSettings();
        }
        if (Input.GetKeyDown(Controls.GetControl("pause")))
        {
            if (isOpen)
            {
                ToggleWindow(currentOpenWindow, true);
                openerSound.Play();
            }
            else
            {
                GameManager.ToggleTime();
            }
            
        }
        // foreach (KeyValuePair<WindowNames, GameObject> item in windows)
        // {
        //     foreach (Transform child in item.Value.transform)
        //     {
        //         // child.gameObject.SetActive(true);
        //         Help.print(currentOpenWindow, child.gameObject, child.gameObject.activeSelf);
        //         if (child.gameObject.activeSelf)
        //         {
        //             Help.print(currentOpenWindow, child.gameObject, child.gameObject.activeSelf);
        //         }
        //     }
        // }
    }
    private void CloseAll()
    {
        foreach (KeyValuePair<WindowNames, GameObject> window in windows)
        {
            ToggleWindow(window.Key, true);
        }
    }

    /// <summary>
    /// Toggles a window based on the root game object which holds the window
    /// It will disable the window and re-enable every child under the root
    /// </summary>
    /// <param name="window">window to toggle</param>
    /// <param name="forceClose">If true, it will close the window instead of toggling</param>
    private void ToggleWindow(WindowNames window, bool forceClose=false)
    {
        if (forceClose)
        {
            foreach (Transform child in windows[window].transform)
            {
                child.gameObject.SetActive(false);
            }
            currentOpenWindow = WindowNames.none;
        }
        else
        {
            if (currentOpenWindow == window)
            {
                currentOpenWindow = WindowNames.none;
            }
            else
            {
                currentOpenWindow = window;
            }
            foreach (KeyValuePair<WindowNames, GameObject> item in windows)
            {
                if (item.Key == currentOpenWindow)
                {
                    foreach (Transform child in item.Value.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                else
                {
                    foreach (Transform child in item.Value.transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
        CheckPause();
    }

    /// <summary>
    /// Accesses the GameManager force time toggle
    /// </summary>
    private void CheckPause()
    {
        if (currentOpenWindow != WindowNames.none)
        {
            GameManager.PauseTime();
            GameManager.cameraZoomOn = false;
        }
        else
        {
            GameManager.ResumeTime();
            GameManager.cameraZoomOn = true;
        }
    }

    /// <summary>
    /// Opens the inventory with the other inventory panel instead of the equipment panel
    /// </summary>
    /// <param name="other">set it to true</param>
    public void Inventory(bool other)
    {
        if (other)
        {
            InventoryPanelManager.EnablePanel("OtherInventoryPanel");
            // InventoryPanelManager.DisablePanel("EquiptmentPanel");
            InventoryPanelManager.EnablePanel("EquiptmentPanel");
            ToggleWindow(WindowNames.inventory);
            GameManager.player.GetComponent<PlayerUIScript>().StopInventoryFlash();
        }
        else
        {
            Inventory();
        }
    }

    /// <summary>
    /// Button toggle Inventory window
    /// </summary>
    public void Inventory()
    {
        InventoryPanelManager.EnablePanel("EquiptmentPanel");
        InventoryPanelManager.DisablePanel("OtherInventoryPanel");
        ToggleWindow(WindowNames.inventory);
        GameManager.player.GetComponent<PlayerUIScript>().StopInventoryFlash();
    }

    /// <summary>
    /// Button toggle SpellBook window
    /// </summary>
    public void SpellBook()
    {
        ToggleWindow(WindowNames.spellBook);
        GameObject.Find("SpellBookHolder").GetComponent<SpellBook>().OpenCloseSpellBook();
    }

    /// <summary>
    /// Button toggle States window
    /// </summary>
    public void Stats()
    {
        ToggleWindow(WindowNames.stats);
        if (statsText == null)
        { 
            statsText = GameObject.Find("StatsText").GetComponent<Text>();
        }
        statsText.text = GameManager.player.GetComponent<PlayerUIScript>().StatsDisplay();
    }

    // this changes from current to completed quests and vice versa

    public void QuestTypeChange ()
    {
        questTypeCurrent = !questTypeCurrent;

        JournalUpdate();
    }


    /// <summary>
    /// Button toggle Journal window
    /// </summary>
    public void Journal()
    {
        // bool journalEmpty = true;
        ToggleWindow(WindowNames.journal);
        if(isOpen)
        {
            currentQuests = GameObject.Find("CurrentQuestsText").GetComponent<Text>();
            completedQuests = GameObject.Find("CompletedQuestsText").GetComponent<Text>();
            questTypeChangeButton = GameObject.Find("QuestTypeChangeButton").GetComponent<Button>();
            GameManager.player.GetComponent<PlayerUIScript>().StopJournalFlash();
        }
        JournalUpdate();
    }

    private void JournalUpdate()
    {
        if (questTypeCurrent == true)
        {
            currentQuests.text = "";
            completedQuests.text = "";
            questTypeChangeButton.GetComponentInChildren<Text>().text = "Completed Quests";
            currentQuests.text += "Current Quests: " + "\n";
            for (int i = 0; i < QuestManager.ListOfQuests.Count; i++)
            {
                Quest thisQuest = QuestManager.ListOfQuests[i];
                if (thisQuest.Accepted == true && thisQuest.Completed == false)
                {
                    currentQuests.text += (" - " + thisQuest.Name + ": " + thisQuest.Description + "\n");
                }
            }
        }
        if (questTypeCurrent == false)
        {
            currentQuests.text = "";
            completedQuests.text = "";
            completedQuests.text += "Completed Quests: " + "\n";
            questTypeChangeButton.GetComponentInChildren<Text>().text = "Current Quests";
            for (int i = 0; i < QuestManager.ListOfQuests.Count; i++)
            {
                Quest thisQuest = QuestManager.ListOfQuests[i];
                if (thisQuest.Completed == true)
                {
                    completedQuests.text += (" - " + thisQuest.Name + ": " + thisQuest.Description + "\n");
                }
            }
        }
    }

    /*public void HelpMenuSwitch()
    {
            HelpMenu();

    }*/

    public void ExtraHelpMenuSwitch()
    {
        ExtraHelpMenu();

    }

    /// <summary>
    /// Button Toggle Settings window
    /// </summary>
    public void Settings() //helpPanelToOptions
    {
        ToggleWindow(WindowNames.settings);
        if (isOpen)
        {
            turnHelpPanelOn = GameObject.Find("HelpButton").GetComponent<Button>();
        }
    }
    public void DebugSettings()
    { 
        ToggleWindow(WindowNames.debug);
    }
    /*void HelpMenu()
    {
        ToggleWindow(WindowNames.help);
        if (isOpen)
        {
            helpPanelToOptions = GameObject.Find("BackSettingsButton").GetComponent<Button>();
            helpPanelToExtraHelp = GameObject.Find("ExtraHelpButton").GetComponent<Button>();
        }
    }*/

    void ExtraHelpMenu()
    {
        ToggleWindow(WindowNames.extraHelp);
        if (isOpen)
        {
            extraHelpPanelToHelp = GameObject.Find("BackToHelpButton").GetComponent<Button>();
        }
        else
        { 
            
        }
    }

}
