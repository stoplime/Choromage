using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCampfire : MonoBehaviour
{
    ItemInserter giveTorch = new ItemInserter();
    public Sprite fire;
    GameObject spriteLoc;
    // Start is called before the first frame update

    bool givenTorch;
    void Start()
    {
        giveTorch.ItemsToInsert.Add(new ItemSaveName("Torch", 1));
        spriteLoc = GameObject.Find("CampfireImage");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnTriggerEnter (Collider collider)
    // {
    //     print("Trigger fireball");
    //     if (collider.gameObject.name == "Fireball")
    //     {
    //         print("Detected fireball");
    //         giveTorch.InsertItems();
    //     }
    // }
    public void TakeDamage(Dictionary<Element, float> elems)
    {
        if (!givenTorch)
        {
            GameObject thorin = GameObject.Find("ThorinIntro");
            SpriteRenderer sr;
            sr = spriteLoc.GetComponent<SpriteRenderer>();
            sr.sprite = fire;
            givenTorch = true;
            // print("Trigger fireball");
            giveTorch.InsertItems();

            // thorin.GetComponent<NPCDialogueDetector>().StartTalking(thorin.name);
        }
    }
    public void TakeDamage(float amount)
    {
        print("Trigger fireball");
        if (!givenTorch)
        {
            GameObject thorin = GameObject.Find("ThorinIntro");
            givenTorch = true;
            giveTorch.InsertItems();
            // thorin.GetComponent<NPCDialogueDetector>().StartTalking(thorin.name);
        }
    }
}
