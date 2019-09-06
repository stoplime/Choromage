using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour, DialogueForest.IClient {

    GameObject dialogBox;
    GameObject optionsPanel;
    //GameObject spellBar;
    Text dialog;
    //GameObject choicePrefab;
    List<GameObject> currentChoices = new List<GameObject>();
    DialogueForest currentTree;
    DialogueForest.Node currentNode;

    Dictionary<string, string> variables;

    
    public bool isTalking;
    GameObject NPC;

    public Dictionary<string, string> Variables { get => variables; set => variables = value; }

    // Use this for initialization
    void Awake () {
        dialogBox = gameObject;
        dialog = GetComponentInChildren<Text>();
        //choicePrefab = Resources.Load("OptionButton") as GameObject;
        //spellBar = GameObject.Find("Hotbar2");
        if (variables == null)
        {
            variables = new Dictionary<string, string>();
        }
        optionsPanel = GameObject.Find("Dialog Options");
        // GameManager.DialogueBox=this;
        //dialogBox.SetActive(false);
    }

    void Update() {
        // Make sure that the game is paused when talking
        if (!GameManager.IsPaused && isTalking)
        {
            GameManager.PauseTime();
        }
    }

    void SetStuff()
    {
        dialogBox = gameObject;
        dialog = GetComponentInChildren<Text>(); 
        //spellBar = GameObject.Find("Hotbar2");
        variables = new Dictionary<string, string>();
        optionsPanel = GameObject.Find("Dialog Options");
    }

    public void StartConversation(string s, GameObject talker)
    {
        //get tree
        NPC = talker;
        if (dialogBox == null)
        { 
            SetStuff();
        }
        dialogBox.SetActive(true);
        //spellBar.SetActive(false);
        currentTree = DialogueManager.AllDialogues[s];
        currentTree.Execute(currentTree.GetFirstNode(), this);
        GameManager.PauseTime();
        isTalking = true;
    }
    public void EndConversation()
    {
        // print("end");
        isTalking = false;
        //destroys prefabs and hides dialog box
        foreach (GameObject g in currentChoices)
        {
            Destroy(g);
        }
        dialogBox.SetActive(false);
        if (NPC.GetComponent<NPCDialogueDetector>() != null)
        {
            NPC.GetComponent<NPCDialogueDetector>().FaceBack();
        }
        //spellBar.SetActive(true);
        GameManager.ResumeTime();
    }
    void LoadDialogText(DialogueForest.Node node)
    {
        string actor = node.actor;
        // Help.print("actor", "\'"+actor+"\'");
        if (actor == "")
        {
            actor = currentTree.GetFirstNode().actor.Replace("!", string.Empty);
        }
        dialog.text = actor + ": " + node.name;    //~~~
    }
    void LoadDialogOptions(IEnumerable<DialogueForest.Node> choices)
    {
        int optionNumber = 1;
        if(optionsPanel==null)
        {
            optionsPanel = GameObject.Find("Dialog Options");
        }
        
        foreach (DialogueForest.Node choice in choices)
        {
            GameObject option = Instantiate(Resources.Load("OptionButton") as GameObject);
            // option.transform.SetParent(optionsPanel.transform);
            option.transform.SetParent(optionsPanel.transform,false);
            option.GetComponentInChildren<Text>().text = optionNumber +". " + choice.name;
            // option.GetComponent<RectTransform>().sizeDelta = new Vector2(50f + choice.name.Length,25f);
            option.name = choice.title;
            currentChoices.Add(option);
            option.GetComponent<DialogOption>().KeyVal = choice.id;
            optionNumber++;
        }
    }
    public void ChoseOption(string i)
    {
        // Debug.Log(i);
        // foreach (KeyValuePair <string,string> k in variables)
        // {
        //     Help.print(k.Key, k.Value);
        // }
        //DialogueNode next;
        foreach (GameObject g in currentChoices)
        {
            Destroy(g);
        }
        DialogueForest.Node chosenNode = currentTree[i];
        if (chosenNode != null)
        {
            // LoadDialogText(chosenNode);
            DialogueForest.Node nextNode = currentTree[chosenNode.next];
            if (nextNode == null)
            {
                EndConversation();
            }
            else{
                currentTree.Execute(nextNode, this);
            }
        }
        else
        {
            //StartStopConversation();
            EndConversation();
        }
    }

    /// <summary>
    /// Runs everytime a node on the json is traversed
    /// </summary>
    /// <param name="node"></param>
    public void Visit(DialogueForest.Node node)
    {
        // print(node.type);
        // if (node.type == "Node" && node.next == null)
        // {
        //     EndConversation();
        // }
        for (int i = 0; i < QuestManager.ListOfQuests.Count; i++)
        {
            Quest thisQuest = QuestManager.ListOfQuests[i];
            // Check if the player has goal_incomplete
            if (thisQuest.Status && thisQuest.Accepted)
            {
                GameManager.DialogueBox.Variables["QuestStatus_" + thisQuest.ID] = "goal_incomplete";
            }
        }
    }

    /// <summary>
    /// Sets text to the screen
    /// </summary>
    /// <param name="node"></param>
    /// <param name="level">The level increments every text node</param>
    public void Text(DialogueForest.Node node, int level)
    {
        LoadDialogText(node);
    }

    /// <summary>
    /// Reads in a list of choices
    /// </summary>
    /// <param name="node"></param>
    /// <param name="choices"></param>
    public void Choice(DialogueForest.Node node, IEnumerable<DialogueForest.Node> choices)
    {
        LoadDialogOptions(choices);
    }

    /// <summary>
    /// Set a variable
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Set(string key, string value)
    {
        variables[key] = value;

        // check quests
        bool targetsHasChanged = false;
        for (int i = 0; i < QuestManager.ListOfQuests.Count; i++)
        {
            Quest thisQuest = QuestManager.ListOfQuests[i];
            foreach (KeyValuePair<string, string> dialogueVar in thisQuest.DialogueRequirements)
            {
                if (variables.ContainsKey(dialogueVar.Key) && variables[dialogueVar.Key] == dialogueVar.Value)
                {
                    thisQuest.Goals["Goal_" + dialogueVar.Key] = true;
                }
            }
            string questStatusName = "QuestStatus_" + thisQuest.ID;
            if (variables.ContainsKey(questStatusName) && variables[questStatusName] == "accepted_incomplete" && !thisQuest.Accepted)
            {
                thisQuest.Accepted = true;
                GameManager.player.GetComponent<PlayerUIScript>().JournalButtonFlash();
            }
            if (variables.ContainsKey(questStatusName) && variables[questStatusName] == "quit_incomplete" && thisQuest.Accepted)
            {
                thisQuest.Accepted = false;
            }
            // Can only complete if the status is true and QuestStatus is "goal_incomplete"
            if (variables.ContainsKey(questStatusName) && variables[questStatusName] == "goal_incomplete" && !thisQuest.Completed && thisQuest.Accepted && thisQuest.Status)
            {
                thisQuest.Completed = true;
                // variables[questStatusName] = "goal_complete";
            }
            if (thisQuest.Completed && thisQuest.TakeItemRequirements)
            {
                for (int j = 0; j < thisQuest.ItemRequirements.Count; j++)
                {
                    ItemSaveName questItem = thisQuest.ItemRequirements[j];
                    for (int k = 0; k < questItem.count; k++)
                    {
                        GameManager.playerInventory.RemoveItem(questItem.item);
                    }
                }
            }
            
            // Checks if the Minimap Target Indicator needs to change
            string targetChange = "QuestTarget_" + thisQuest.ID + "_";
            for (int j = 0; j < thisQuest.Targets.Count; j++)
            {
                string targetChangeName = targetChange + thisQuest.Targets[j].TargetName;
                if (variables.ContainsKey(targetChangeName))
                {
                    if (variables[targetChangeName] == "true")
                    {
                        thisQuest.Targets[j].Enable = true;
                        targetsHasChanged = true;
                    }
                    else if (variables[targetChangeName] == "false")
                    {
                        thisQuest.Targets[j].Enable = false;
                        targetsHasChanged = true;
                    }
                }
            }
        }
        if (targetsHasChanged)
        {
            MinimapIndicatorManager.UpdateMinimapIndicator();
        }

        // check item inserts
        for (int i = 0; i < ItemInsertExtractManager.ItemsInsert.Count; i++)
        {
            ItemInserter thisInserter = ItemInsertExtractManager.ItemsInsert[i];
            if (variables.ContainsKey(thisInserter.VariableFlag) && variables[thisInserter.VariableFlag] == "true")
            {
                // Help.print("Inserted this item", thisInserter.ItemsToInsert[0].item);
                thisInserter.InsertItems();
                variables[thisInserter.VariableFlag] = "false";
            }
        }
        // check item extracts
        for (int i = 0; i < ItemInsertExtractManager.ItemsExtract.Count; i++)
        {
            ItemExtractor thisExtractor = ItemInsertExtractManager.ItemsExtract[i];
            if (variables.ContainsKey(thisExtractor.VariableFlag) && variables[thisExtractor.VariableFlag] == "true")
            {
                thisExtractor.ExtractItems();
                variables[thisExtractor.VariableFlag] = "false";
            }
        }
        // Check for unlocked spells
        if (variables.ContainsKey("SpellUnlock") && variables["SpellUnlock"] != "null")
        {
            string spellName = variables["SpellUnlock"];
            variables["SpellUnlock"] = "null";
            SpellManager.UnlockNewSpell(spellName);
        }
        // if (variables.ContainsKey("InTutorial"))
        if (key == "InTutorial")
        {
            GameManager.InTutorial = false;
        }

        if (key == "ClearAllMinimapTargets")
        {
            MinimapIndicatorManager.ClearAllTargets();
            MinimapIndicatorManager.enable = false;
        }
        if (!variables.ContainsKey(key))
        {
            variables.Add(key, value);
        }
    }

    /// <summary>
    /// gets a variable
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Get(string key)
    {
        if (variables.ContainsKey(key))
        {
            return variables[key];
        }
        return null;
    }
}

