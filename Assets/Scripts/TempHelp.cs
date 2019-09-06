using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempHelp : MonoBehaviour
{
    //public Scrollbar controllsTextHere;
    public Text controlsText;
    GameObject gameManager;
    List<string> currentControls;
    List<KeyCode> currentKeys;
    void Start()
    {
        currentControls = Controls.GetCodes();
        // print(currentControls);
        currentKeys = new List<KeyCode>();
        //24
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
            { controlsText.text += ": Left Click"; }
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
            /*if (i > 15)
            {
                currentKeys.Add(Controls.GetControl(code));
                controlsText.text += ("\n" + " " + currentKeys[currentKeys.Count - 1].ToString());
            }*/
            /*currentKeys.Add(Controls.GetControl(code));
            Debug.Log(code);
            controlsText.text += (" " + currentKeys[currentKeys.Count - 1].ToString() + "\n");*/
        }
        /*foreach (string code in currentControls)
        {
            controlsText.text += code;
            currentKeys.Add(Controls.GetControl(code));
            Debug.Log(code);
            controlsText.text += (" " + currentKeys[currentKeys.Count - 1].ToString() + "\n");

        }*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}

