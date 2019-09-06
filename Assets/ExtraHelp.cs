using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraHelp : MonoBehaviour
{
    Image topBarArrow;
    Image healthArrow;
    Image manaArrow;
    Image lowerBarArrow;
    Image miniMapArrow;
    Button topBarInfo;
    Button healthBarInfo;
    Button manBarInfo;
    Button lowerBarInfo;
    Button miniMapInfo;
    GameObject controls;
    Text descriptionText;

    Text controlsText;
    GameObject gameManager;
    List<string> currentControls;
    List<KeyCode> currentKeys;
    bool doOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        currentControls = Controls.GetCodes();
        // print(currentControls);
        currentKeys = new List<KeyCode>();
        GetControlsText();
        OnEnable();
        // HideArrows();
    }

    //TODO: find a better way to do this    ps Dylan why didn't you use a switch case?
    void GetControlsText()
    { 
        for (int i = 0; i < currentControls.Count - 12; i++)
        {
            string code = currentControls[i];
            controlsText.text += "\n" + code;
            
            if (i == 0)
            { controlsText.text += ": Up Arrow"; }
            if (i == 1)
            { controlsText.text += ": Down Arrow"; }
            if (i == 2)
            { controlsText.text += ": Right Arrow"; }
            if (i == 3)
            { controlsText.text += ": Left Arrow"; }
            if (i == 4)
            { controlsText.text += ": Middle Mouse Button"; }
            if (i == 5)
            { controlsText.text += ": W"; }
            if (i == 6)
            { controlsText.text += ": S"; }
            if (i == 7)
            { controlsText.text += ": D"; }
            if (i == 8)
            { controlsText.text += ": A"; }
            if (i == 9)
            { controlsText.text += ": Shift"; }
            if (i == 10)
            { controlsText.text += ": Spacebar"; }
            if (i == 11)
            { controlsText.text += ": Back Quotation"; }
            if (i == 12)
            { controlsText.text += ": 1"; }
            if (i == 13)
            { controlsText.text += ": 2"; }
            if (i == 14)
            { controlsText.text += ": 3"; }
            if (i == 15)
            { controlsText.text += ": 4"; }
            if (i == 16)
            { controlsText.text += ": 5"; }
            if (i == 17)
            // { controlsText.text += ": Left Click"; }
            { controlsText.text += ": 6"; }
            if (i == 18)
            { controlsText.text += ": Left Click"; }
            if (i == 19)
            { controlsText.text += ": E"; }
            if (i == 20)
            { controlsText.text += ": B"; }
            if (i == 21)
            { controlsText.text += ": C"; }
            if (i == 22)
            { controlsText.text += ": J"; }
            if (i == 23)
            { controlsText.text += ": Escape"; }
            if (i == 24)
            { controlsText.text += ": T"; }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        //helpPanelToExtraHelp = GameObject.Find("ExtraHelpButton").GetComponent<Button>();
        if (doOnce == false)
        {
            controlsText = GameObject.Find ("ControlHelpText").GetComponent<Text>();
            controls = GameObject.Find("ControlsScroll");
            topBarArrow = GameObject.Find("TopBarArrow").GetComponent<Image>();
            healthArrow = GameObject.Find("HealthArrow").GetComponent<Image>();
            manaArrow = GameObject.Find("ManaArrow").GetComponent<Image>();
            lowerBarArrow = GameObject.Find("LowerBarArrow").GetComponent<Image>();
            miniMapArrow = GameObject.Find("MiniMapArrow").GetComponent<Image>();
            doOnce = true;
        }
        controls.SetActive(false);
        topBarArrow.enabled = false;
        healthArrow.enabled = false;
        manaArrow.enabled = false;
        lowerBarArrow.enabled = false;
        miniMapArrow.enabled = false;

        /*topBarInfo = GameObject.Find("TopBarButton").GetComponent<Button>();
        healthBarInfo = GameObject.Find("HealthButton").GetComponent<Button>();
        manBarInfo = GameObject.Find("ManaButton").GetComponent<Button>();
        lowerBarInfo = GameObject.Find("LowerBarButton").GetComponent<Button>();
        miniMapInfo = GameObject.Find("MiniMapButton").GetComponent<Button>();*/

        descriptionText = GameObject.Find("ExtraHelpText").GetComponent<Text>();
        descriptionText.text = "";
    }
    void OnDisable()
    {
        ClearArrows();
    }
    public void TopBarButtonSwitch ()
    {
        ClearArrows();
        topBarArrow.enabled = true;
        descriptionText.text = "From left to right this bar of buttons gives access to the player inventory, spell book, journal, and settings. " +
            "The inventory shows what your character has in their bag. Some items may be equiped by dragging to an equipment slot on the left. " +
            "The spell book allows you to drag spells you have access to down to the lower bar. " +
            "The journal shows current quests (your objectives in the game) and completed quests. " +
            "The settings allow you to save and quit or access this menu.";
    }

    public void HealthButtonSwitch()
    {
        ClearArrows();
        healthArrow.enabled = true;
        descriptionText.text = "This green bar is your healthbar. When enemies attack you, you will lose health. If your health reaches zero you lose the game however you may heal with " +
            "the life spell (if it is unlocked) or wait for your health to recover naturally over time.";
    }

    public void ManaButtonSwitch()
    {
        ClearArrows();
        manaArrow.enabled = true;
        descriptionText.text = "The blue bar is your manabar. When you cast a spell, you will lose some mana. Mana replenishes over time slowly.";
    }

    public void LowerBarSwitch()
    {
        ClearArrows();
        lowerBarArrow.enabled = true;
        descriptionText.text = "This is your skillbar. To use a the water, life, or death spell you simply click it and it will cast. " +
            "For the other spells you must click the button then select the target you wish the spell to hit. " +
            "If you do not have enough mana to cast a spell then you will have to wait for it to naturally replenish." +
            "Each time you cast a spell you must wait until that spell's cooldown time has finished counting down (shown on the bottom of each spell symbol." +
            "You may also customize this bar by using the spell book and drag an icon onto the bar wherever you want.";
    }
    public void MiniMapSwitch()
    {
        ClearArrows();
        miniMapArrow.enabled = true;
        descriptionText.text = "This is a minimap that will show you the surrounding area. Enemies will show up as red. " +
            "A green arrow will show you the direction to complete your current quest.";
    }

    public void ControlsSwitch()
    {
        ClearArrows();
        controls.SetActive(true);
    }

    void ClearArrows()
    {
        descriptionText.text = "";
        topBarArrow.enabled = false;
        healthArrow.enabled = false;
        manaArrow.enabled = false;
        lowerBarArrow.enabled = false;
        miniMapArrow.enabled = false;
        controls.SetActive(false);
    }

}

