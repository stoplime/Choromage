using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSaver{
    public Dictionary<string, string> Variables;

    public void SaveFromDialogBox()
    {
        Variables = GameManager.DialogueBox.Variables;
    }

    public void LoadToDialogBox()
    {
        GameManager.DialogueBox.Variables = Variables;
    }
}

