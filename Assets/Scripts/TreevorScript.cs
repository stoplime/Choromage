using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreevorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Change the cube color to green.
        if (other.gameObject.tag == "Player" && GameManager.DialogueBox.Variables.ContainsKey("HasTalkedToTreevor") &&
            GameManager.DialogueBox.Variables["HasTalkedToTreevor"] == "GivesThorinsMessage")
        {
            //transform.parent.GetComponent<NPCDialogueDetector>().StartTalking(gameObject.name);
            GameManager.DialogueBox.StartConversation("Treevor",gameObject);
        }
    }

}
