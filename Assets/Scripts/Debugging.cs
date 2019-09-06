using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugging : MonoBehaviour {

    // Use this for initialization
    GameObject debugButton;
    GameObject debugTextArea;
    GameObject debugParent;
    Text debuggingText;
    private bool showDebugText;
    bool godModeToggling;
    Slider speedSlider;

    private Dictionary<string, Toggle> debugToggles = new Dictionary<string, Toggle>();

    public enum ControlEnum
    {
		godSpeed,
		intagible,
		debugText,
		freeSpells,
		fastCasting,
		endlessHealth,
		customManaRegen,
		customHealthRegen,
		immortal,
		invulnerable,
        invisible
        //fastSpells,
        //powerfulSpells
    }

    void Awake()
    {
        debugParent = GameObject.Find("DebugSettingsPanel").transform.Find("Panel").gameObject;
    }

    void Start () {
        debugButton = GameObject.Find("DebugOptionsButton");
        if (GameObject.Find("DebugTextArea") != null)
        {
            debugTextArea = GameObject.Find("DebugTextArea");
            debuggingText = GameObject.Find("Debugger").GetComponent<Text>();
            debuggingText.text = "";
        }
        debugButton.SetActive(GameManager.GodMode);
		
        CreateToggles();
		
    }

    void CreateToggles()
    { 
		debugToggles.Add("godSpeed", GameManager.FindObject(debugParent, "GodSpeedToggle").GetComponent<Toggle>());
		debugToggles.Add("intagible", GameManager.FindObject(debugParent, "IntangibleToggle").GetComponent<Toggle>());
		debugToggles.Add("debugText", GameManager.FindObject(debugParent, "DebugTextToggle").GetComponent<Toggle>());
		debugToggles.Add("freeSpells", GameManager.FindObject(debugParent, "FreeSpellsToggle").GetComponent<Toggle>());
		debugToggles.Add("fastCasting", GameManager.FindObject(debugParent, "FastCastingToggle").GetComponent<Toggle>());
		debugToggles.Add("endlessHealth", GameManager.FindObject(debugParent, "EndlessHealthToggle").GetComponent<Toggle>());
		debugToggles.Add("customManaRegen", GameManager.FindObject(debugParent, "CustomManaRegenToggle").GetComponent<Toggle>());
		debugToggles.Add("customHealthRegen", GameManager.FindObject(debugParent, "CustomHealthRegenToggle").GetComponent<Toggle>());
		debugToggles.Add("immortal", GameManager.FindObject(debugParent, "ImmortalToggle").GetComponent<Toggle>());
		debugToggles.Add("invulnerable", GameManager.FindObject(debugParent, "InvulnerableToggle").GetComponent<Toggle>());
        debugToggles.Add("invisible", GameManager.FindObject(debugParent, "InvisibleToggle").GetComponent<Toggle>());
        //debugToggles.Add("fastSpells", GameManager.FindObject(debugParent, "FastSpellToggle").GetComponent<Toggle>());
        //debugToggles.Add("powerfulSpells", GameManager.FindObject(debugParent, "").GetComponent<Toggle>());
		debugTextArea.SetActive(GameManager.GodMode&&debugToggles["debugText"].isOn);
		debugToggles["godSpeed"].gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.P) &&!godModeToggling)
        {
            godModeToggling = true;
            GameManager.GodMode = !GameManager.GodMode;
            debugButton.SetActive(GameManager.GodMode);
			debugTextArea.SetActive(GameManager.GodMode&&debugToggles["debugText"].isOn);
        }
        if (Input.GetKeyUp(KeyCode.O) || Input.GetKeyUp(KeyCode.P))
        {
            godModeToggling = false;
        }
		if (GameManager.GodMode)
		{
			CheckToggles();	
		}
    }

    void CheckToggles()
    {
        foreach (string key in debugToggles.Keys)
        {
            // if (Input.GetKey(KeyCode.F5))
            // {
            //     Help.print(Controls.GetControl(key), debugToggles[key], debugToggles[key].interactable);
            // }
            if (Input.GetKeyDown(Controls.GetControl(key))&&debugToggles[key].interactable)
            {
                debugToggles[key].isOn = !debugToggles[key].isOn;
            }
        }
    }

    public void UpdateDebuggerText(string debugText)
    {
        if (debugTextArea != null)
        {
            debuggingText.text = debugText;
        }
    }

    public void ToggleGodSpeed(Toggle speedToggle)
    {
        GameManager.GodSpeed = speedToggle.isOn;
        GameManager.GodSpeedMultiplier = (int)speedToggle.transform.GetComponentInChildren<Slider>().value;
    }
    public void ToggleIntangibility(Toggle intagibleToggle)
    {
        GameManager.Intangible = intagibleToggle.isOn;
        //print(GameManager.player.GetComponent<Collider>());
        GameManager.player.GetComponent<Collider>().enabled = !GameManager.Intangible;
    }
    public void SetGodSpeed(Slider newSpeed)
    {
        GameManager.GodSpeedMultiplier = (int)newSpeed.value;
    }

    public void ShowHideDebugText(Toggle toggleText)
    {
		
        if (debugTextArea != null)
        { 
			debugTextArea.SetActive(GameManager.GodMode&&debugToggles["debugText"].isOn);
		}
    }

    public void ToggleSpellCost(Toggle freeSpell)
    {
        GameManager.FreeSpells = freeSpell.isOn;
    }

    public void ToggleImmortality(Toggle immortality)
    {
        GameManager.Immortal = immortality.isOn;
    }
    public void ToggleInvisible(Toggle invisible)
    {
        GameManager.Invisible = invisible.isOn;
    }
    public void ToggleFastCast(Toggle fastCasting)
    {
        GameManager.FastCasting = fastCasting.isOn;
    }
    public void SpawnNewEnemy(Dropdown newEnemyValue)
    {
        if (newEnemyValue.value != 0)
        {
            GameObject newEnemy = Instantiate(EnemyManager.Enemies[newEnemyValue.value - 1], RandomSpaceNearPlayer(), transform.rotation, GameObject.Find("Enemies").transform);
            newEnemyValue.value = 0;
        }

    }

    Vector3 RandomSpaceNearPlayer()
    {
        //Vector3 randomPosition = 
        Vector2 randomCirclePos = Random.insideUnitCircle * 25;
        Vector3 randomPosition = new Vector3(GameManager.PlayerPos.x + randomCirclePos.x, GameManager.PlayerPos.y, GameManager.PlayerPos.z +randomCirclePos.y);
        return randomPosition;
    }
}
