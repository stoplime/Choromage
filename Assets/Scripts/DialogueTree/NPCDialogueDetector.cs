using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueDetector : MonoBehaviour {
    public bool ShouldFacePlayer = true; 
    GameObject player;
    // Vector3 startRotation;
    Quaternion startRotation;
    // Use this for initialization
    void Awake () {
	    player = GameObject.Find("Player");
        // startRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,transform.eulerAngles.z);
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update () {
        //if space pressed when player nearby && player not already talking
        if (Input.GetKeyDown(Controls.GetControl("talk")) &&
            Vector3.Distance(player.transform.position, transform.position)<5 &&
            !GameManager.DialogueBox.gameObject.activeSelf)
	    {
            StartTalking(gameObject.name);
        }
    }

    public void ObjectClicked(RaycastHit hit)
    {
        if ((hit.transform.tag == "NPC" || hit.transform.tag == "NPC Caster"))
        {
            StartTalking(hit.transform.gameObject.name);
        }
    }

    public void StartTalking(string dialogName)
    { 
        GameManager.DialogueBox.StartConversation(dialogName, gameObject);
        if (ShouldFacePlayer)
        {
            FacePlayer();
        }
    }
    void FacePlayer()
    {
        Vector3 tempPlayerPos = GameManager.PlayerPos;
        tempPlayerPos.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation(tempPlayerPos- transform.position);
    }
    public void FaceBack()
    {
        transform.rotation = startRotation;
    }
}
