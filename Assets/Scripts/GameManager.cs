using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public static GameObject player;
	//public static GameObject deathScreen;
	public static Inventory playerInventory;
	public static Inventory playerEquipments;
    private static float playerSpeed = 300f;
    private static float sprintSpeed = 600f;
    private static bool sprinting;
    static bool inTutorial = true;
    private static float lastSaveTime = 0;
	// How long before the game auto saves in seconds
	private static float saveInterval = 60;

    private static GameObject cantis;
	
	public static DialogBox DialogueBox;

    public static bool MuteSoundeEffects;

    #region 
    //god mode
    private static bool godMode =false;
    private static bool godSpeed;
	private static int godSpeedMultiplier;
	/// <summary>
    /// run through walls
    /// </summary>
	private static bool intagible;
	/// <summary>
	/// wheter or not player's spells cost mana
	/// </summary>
    private static bool freeSpells;
	/// <summary>
	/// huge mana pool. to test spells and mana cost
	/// </summary>
    private static bool fastCasting;
	private static bool endlessHealth;
    private static bool customManaRegen;
    private static int customManaRegenFactor;
    private static bool customHealthRegen;
	private static int customHealthRegenFactor;
 
	/// <summary>
	/// will not call player death even when hp = 0
	/// </summary>
    private static bool immortal;
    /// <summary>
    /// never gets hurt when take damage is called
    /// </summary>
    private static bool invulnerable;
    private static bool invisible;

    private static Ray ray;
    private static RaycastHit hit;

    //if in a menu will be false
    public static bool cameraZoomOn = true;

	public static bool GodMode
    {
        get { return godMode; }
        set { godMode = value; }
    }
	public static bool GodSpeed
    {
        get { return godSpeed && godMode; }
		//get { return godSpeed; }
        set { godSpeed = value; }
    }
	public static int GodSpeedMultiplier
    {
		get { 
				if (godSpeedMultiplier == 0)
				{
					godSpeedMultiplier = 1;
				}
				return godSpeedMultiplier;
			}
        set { godSpeedMultiplier = value; }
    }
	public static bool Intangible
    { 
		get { return intagible && godMode; }
        set { intagible = value; }
	}
	public static bool FreeSpells
    { 
		get { return freeSpells  && godMode; }
        set { freeSpells = value; }
	}
	public static bool FastCasting
    { 
		get { return fastCasting && godMode; }
        set { fastCasting = value; }
	}
	public static bool CustomManaRegen
    { 
		get { return customManaRegen && godMode; }
        set { customManaRegen = value; }
	}
	public static int CustomManaRegenFactor
    { 
		get { return customManaRegenFactor; }
        set { customManaRegenFactor = value; }
	}
	public static bool CustomHealthRegen
    { 
		get { return customHealthRegen && godMode; }
        set { customHealthRegen = value; }
	}
	public static int CustomHealthRegenFactor
    { 
		get { return customHealthRegenFactor; }
        set { customHealthRegenFactor = value; }
	}

	public static bool EndlessHealth
    { 
		get { return endlessHealth && godMode; }
        set { endlessHealth = value; }
	}
	public static bool Invulnerable
    { 
		get { return invulnerable && godMode; }
        set { invulnerable = value; }
	}

	public static bool Immortal
    { 
		get { return immortal && godMode; }
        set { immortal = value; }
	}

	public static bool Invisible
    { 
		get { return invisible && godMode; }
        set { invisible = value; }
	}

    #endregion



	public static Vector3 PlayerPos
    {
        get { return player.transform.position; }
    }

    public static GameObject Cantis
	{
        get { return cantis; }
        set { cantis = value; }
    }
	public static bool Sprinting
    {
        set { sprinting = value; }
    }

    public static float PlayerSpeed
    {
        get
        {
            if (GodSpeed)
            {
               
                if (sprinting)
                { return sprintSpeed*godSpeedMultiplier*UniversalSpeedMultiplier; }
                else
                { return playerSpeed*godSpeedMultiplier*UniversalSpeedMultiplier; }
            }
            else
            { 
				if (sprinting)
                { return sprintSpeed*UniversalSpeedMultiplier; }
                else
                { return playerSpeed*UniversalSpeedMultiplier; }
			}
        }
    }
    public static float UniversalSpeedMultiplier = 1.25f;

    /// <summary>
    /// Despawns GameObjects such as NPCs, props, and any projectiles/ammo when distance form player is greater than this value. This is especially useful to make sure arrows don't go on forever after it goes offscreen.
    /// </summary>
    public static float despawnDistance = 50;

	private static bool isPaused = false;
	public static bool IsPaused
	{
		get{ return isPaused; }
	}
    public static bool InTutorial { get => inTutorial; set => inTutorial = value; }

	public static bool Killable
    {
        get 
		{
            return !InTutorial;
        }
    }
    /// <summary>
    /// Toggles the time to pause or resume
    /// </summary>
    public static void ToggleTime()
	{
		if (isPaused)
		{
			Time.timeScale = 1;
		}
		else
		{
			Time.timeScale = 0;
		}
		isPaused = !isPaused;
	}

	/// <summary>
	/// Force pause time
	/// Ie if paused then do nothing, if resumed then pause
	/// </summary>
	public static void PauseTime()
	{
		if (!isPaused)
		{
			Time.timeScale = 0;
			isPaused = true;
		}
	}

    /// <summary>
    /// Force resume time
    /// Ie if paused then it will resume, if resumed do nothing
    /// </summary>
    public static void ResumeTime()
	{
		if (isPaused)
		{
			Time.timeScale = 1;
			isPaused = false;
		}
	}

	public static void ResetNewGame()
	{
		ResetGame.EmptySave();
        inTutorial = true;
    }

	 //Awake is always called before any Start functions
	void Awake()
	{
		player = GameObject.Find("Player");
		findPlayerInventory();
		DialogueBox = GameObject.FindGameObjectWithTag("ChatterBox").GetComponent<DialogBox>();
		
		ResetGame.Initialize();
		QuestManager.Initialize();
		EnemyManager.Initialize();
		ItemInsertExtractManager.Initialize();
		MinimapIndicatorManager.Initialize();
	}

	void Start()
	{
		InventorySaver.LoadPlayerInventories();
		// Initialize save
		lastSaveTime = Time.time;
		MinimapIndicatorManager.UpdateMinimapIndicator();
	}

	private static void findPlayerInventory()
	{
		List<Inventory> inventories = new List<Inventory>(player.GetComponents<Inventory>());
		for (int i = 0; i < inventories.Count; i++)
		{
			if (inventories[i].Type == Inventory.InventoryType.PlayerInventory)
			{
				playerInventory = inventories[i];
			}
			else if (inventories[i].Type == Inventory.InventoryType.PlayerEquipment)
			{
				playerEquipments = inventories[i];
			}
		}
	}

	public static void SaveGame()
	{
		SaveManager.CheckSaveFile();
		InventorySaver.SavePlayerInventories();
		SaveManager.SavePlayerInfo();
		SaveManager.SaveQuests();
		SaveManager.SaveDialogue();
        SpellManager.SaveUnlockedSpells();
        print("Game Saved");
    }

	public static void CheckClick()
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            hit.transform.gameObject.SendMessage("ObjectClicked", hit, SendMessageOptions.DontRequireReceiver);
        }
	}

    public void Update()
	{
		// Periodically save the game
		if (Time.time - lastSaveTime >= saveInterval || (Input.GetKey(KeyCode.L) && Input.GetKeyDown(KeyCode.O)))
		{
			lastSaveTime = Time.time;
			SaveGame();
		}
		CheckClick();

		// TODO: Change this to work consistently and also check in-game saving
		// Temporary save of the environment Control Save the environment
		if (SceneManager.GetActiveScene().name == "EarlyEnvironment")
		{
			if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.P))
			{
				print("Saving Tiles!");
				EnvironmentSaver env = new EnvironmentSaver();
				env.SaveEnvironmentData();
				env.SaveTileData();
				env.SaveNPCsData();
				env.SaveInitInventoryData();
				print("Done Saving Tiles!");
			}
		}
        if (Input.GetKeyDown(KeyCode.M))
        {
            MuteSoundeEffects = !MuteSoundeEffects;
        }
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        // 	QuestAttention.AttachParticle("Cassida Smith");
        // }
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        // 	QuestAttention.RemoveParticle("Cassida Smith");
        // }
    }

	public static GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs= parent.GetComponentsInChildren<Transform>(true);
        foreach(Transform t in trs){
            if(t.name == name){
                return t.gameObject;
            }
        }
        return null;
    }
}
