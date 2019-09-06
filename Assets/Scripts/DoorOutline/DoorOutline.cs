using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOutline : MonoBehaviour
{
    Renderer rend;
    bool playerNear = false;
    public bool isIndoors = false;
    GameObject player;
    GameObject indoorsSpawn;
    GameObject outdoorsSpawn;
    static bool outdoors = true;
    public bool indoorsPressedQ;
    public AudioClip doorOpening;
    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    GameObject camera;

    static float timestamp = 0;
    //this is rose quest 1 (the quest to go to the runic circle)
    void Start()
    {
        source = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        player = GameObject.Find("Player");
        //indoorsSpawn = this.gameObject.transform.GetChild(0).gameObject;
        //outdoorsSpawn = this.gameObject.transform.GetChild(1).gameObject;
        // outdoors = true;
        indoorsSpawn = GameObject.Find("ThorinsIndoors");
        outdoorsSpawn = GameObject.Find("ThorinsOutdoors");
        camera = GameObject.Find("CameraControler");
    }

    private void UpdateQuestTargets()
    {
        Quest q = QuestManager.GetQuestByID("ThorinNecromancyQuest");
        if(q.ID != null)
        {
            q.SetTarget("QuestThorinsHouseLoc", false);
        }
        MinimapIndicatorManager.UpdateMinimapIndicator();
    }

    void Update()
    {
        //this is rose quest 1 (the quest to go to the runic circle)
        Quest thisQuest = QuestManager.ListOfQuests[2];
        if (playerNear == true && /* Input.GetKeyDown("q") && */ thisQuest.Completed == true /*&& isIndoors == false*/ && timestamp < Time.time)
        {
            timestamp = Time.time + 1f;
            if (outdoors)
            {
                outdoors = false;
                player.transform.position = indoorsSpawn.transform.position;
                float vol = Random.Range(volLowRange, volHighRange);
                //note readd the source play for music
                //source.PlayOneShot(doorOpening, vol);
                //camera.transform.position = new Vector3(indoorsSpawn.transform.position.x, this.transform.position.y, indoorsSpawn.transform.position.z);
                camera.transform.position = camera.GetComponent<CameraControls>().GetCameraPosFromPlayer();
                UpdateQuestTargets();
                MinimapIndicatorManager.Disable();
            }
            else
            {
                outdoors = true;
                player.transform.position = outdoorsSpawn.transform.position;
                float vol = Random.Range(volLowRange, volHighRange);
                //note readd the source play for music
                //source.PlayOneShot(doorOpening, vol);
                //outdoors = true;
                camera.transform.position = camera.GetComponent<CameraControls>().GetCameraPosFromPlayer();
                MinimapIndicatorManager.Enable();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if ( other.gameObject.tag == "Player")
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
