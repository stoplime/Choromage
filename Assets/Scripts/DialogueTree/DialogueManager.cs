using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// Holds all the dialogue in a static object
    /// </summary>
    /// <typeparam name="string">The name of the dialogue</typeparam>
    /// <typeparam name="DialogueForest">The dialogue parser</typeparam>
    /// <returns></returns>
    public static Dictionary<string, DialogueForest> AllDialogues = new Dictionary<string, DialogueForest>();
    GameObject dialogueBox;

    // Use this for initialization
    void Start()
    {
        dialogueBox = GameObject.Find("Dialog Box");
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }

        DirectoryInfo dilogDirectory = new DirectoryInfo(Application.dataPath + "/StreamingAssets/bin/dialog");
        FileInfo[] dilogFiles = dilogDirectory.GetFiles("*.json"); //Getting Text files
        foreach(FileInfo file in dilogFiles )
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            DialogueForest df = new DialogueForest();
            df.Load(File.ReadAllText(file.FullName));
            AllDialogues[name] = df;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetDialogueVariable(string dialogueID, string key, string value)
    {
        if (AllDialogues.ContainsKey(dialogueID) && AllDialogues[dialogueID].db != null)
        {
            AllDialogues[dialogueID].db.Set(key, value);
        }
    }

    /// <summary>
    /// Provides the list of Dialogue Names
    /// </summary>
    /// <returns></returns>
    public List<string> GetListofDialogues()
    {
        return new List<string>(AllDialogues.Keys);
    }

}
