using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeStaff : MonoBehaviour
{
    private GameObject particles;

    bool charged;
    // Start is called before the first frame update
    void Start()
    {
        particles = GameManager.FindObject(transform.parent.gameObject, "Particles");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // public void OnTriggerEnter(Collider other)
    // {
    //     print(other.gameObject);
    // }
    public void HealDamage(float amount)
    {
        if(GameManager.playerInventory.RemoveItem("Uncharged Staff"))
        {
            GameManager.playerInventory.AddItem("Novice Staff");
            GameManager.player.GetComponent<PlayerUIScript>().InventoryButtonFlash();
            GameManager.DialogueBox.Variables["MeetRose"] = "BeenToRunicCircle";
            Quest q = QuestManager.GetQuestByID("RoseQuest1");
            if(q.ID != null)
            {
                q.SetTarget("QuestRunicCircleLoc", false);
                q.SetTarget("QuestRoseLoc", true);
            }
            MinimapIndicatorManager.UpdateMinimapIndicator();
            particles.SetActive(true);
        }
    }
}

