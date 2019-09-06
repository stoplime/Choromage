using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveDoorOutline : MonoBehaviour
{
    Renderer rend;
    bool playerNear = false;
    public bool isIndoors = false;
    GameObject player;
    GameObject indoorsSpawn;
    GameObject outdoorsSpawn;
    static bool outdoors = true;
    GameObject camera;

    static float timestamp = 0;

    void Start()
    {
        rend = GetComponent<Renderer>();
        player = GameObject.Find("Player");

        //indoorsSpawn = this.gameObject.transform.GetChild(0).gameObject;
        //outdoorsSpawn = this.gameObject.transform.GetChild(1).gameObject;
        indoorsSpawn = GameObject.Find("CaveIndoors");
        outdoorsSpawn = GameObject.Find("CaveOutdoors");
        camera = GameObject.Find("CameraControler");
    }

    private void DisableQuestCaveLoc()
    {
        Quest q = QuestManager.GetQuestByID("DragonQuest2");
        if(q.ID != null)
        {
            q.SetTarget("QuestCaveLoc", false);
        }
        MinimapIndicatorManager.UpdateMinimapIndicator();
    }

    void Update()
    {
        //this is the dragon boss quest
        Quest thisQuest = QuestManager.ListOfQuests[3];
        if (playerNear == true && /* Input.GetKeyDown("q") && */ thisQuest.Accepted == true && timestamp < Time.time)
        {
            timestamp = Time.time + 1f;
            if (outdoors)
            {
                outdoors = false;
                player.transform.position = indoorsSpawn.transform.position;
                //camera.transform.position = new Vector3(indoorsSpawn.transform.position.x, this.transform.position.y, indoorsSpawn.transform.position.z);
                camera.transform.position = camera.GetComponent<CameraControls>().GetCameraPosFromPlayer();
                DisableQuestCaveLoc();
                MinimapIndicatorManager.Disable();
            }
            else
            {
                outdoors = true;
                player.transform.position = outdoorsSpawn.transform.position;
                //outdoors = true;
                camera.transform.position = camera.GetComponent<CameraControls>().GetCameraPosFromPlayer();
                MinimapIndicatorManager.Enable();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rend.material.SetFloat("_OutlineSize", 5);
            playerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rend.material.SetFloat("_OutlineSize", 0);
            playerNear = false;
        }
    }


}