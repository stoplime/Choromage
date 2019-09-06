using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroExitOutline : MonoBehaviour
{
    Renderer rend;
    bool playerNear = false;
    bool readyToLeave = false;
    GameObject player;
    GameObject indoorsSpawn;
    GameObject outdoorsSpawn;
    bool outdoors;
    GameObject camera;
    void Start()
    {
        rend = GetComponent<Renderer>();
        player = GameObject.Find("Player");
        indoorsSpawn = GameObject.Find("RealMapSpawn");
        outdoorsSpawn = GameObject.Find("ForestExit");
        //indoorsSpawn = this.gameObject.transform.GetChild(0).gameObject;
        //outdoorsSpawn = this.gameObject.transform.GetChild(1).gameObject;
        outdoors = true;
        camera = GameObject.Find("CameraControler");
    }

    private void UpdateQuestTargets()
    {
        Quest q = QuestManager.GetQuestByID("FindRose");
        if(q.ID != null)
        {
            q.SetTarget("QuestForestExitLoc", false);
            q.SetTarget("QuestRoseLoc", true);
        }
        MinimapIndicatorManager.UpdateMinimapIndicator();
    }

    void Update()
    {
        if (GameManager.DialogueBox.Variables.ContainsKey("EnableForestExit"))
        {
            readyToLeave = GameManager.DialogueBox.Variables["EnableForestExit"] == "true";
        }
        if (playerNear == true && /* Input.GetKeyDown("q")  && */ readyToLeave == true && GameManager.playerInventory.RemoveItem("Torch"))
        {
            UpdateQuestTargets();
            if (outdoors == true)
            {
                player.transform.position = indoorsSpawn.transform.position;
                //camera.transform.position = new Vector3(indoorsSpawn.transform.position.x, this.transform.position.y, indoorsSpawn.transform.position.z);
                camera.transform.position = camera.GetComponent<CameraControls>().GetCameraPosFromPlayer();
            }
            if (outdoors == false)
            {
                player.transform.position = outdoorsSpawn.transform.position;
                //outdoors = true;
                camera.transform.position = camera.GetComponent<CameraControls>().GetCameraPosFromPlayer();
            }
            outdoors = !outdoors;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (rend == null)
            {
                rend = GetComponent<Renderer>();
            }
            rend.material.SetFloat("_OutlineSize", 5);
            playerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (rend == null)
            {
                rend = GetComponent<Renderer>();
            }
            rend.material.SetFloat("_OutlineSize", 0);
            playerNear = false;
        }
    }


}
