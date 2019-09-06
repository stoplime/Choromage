using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStateMachine :EnemyStateMachine {

    string soundSource ="Sounds/Fantasy Sfx/Mp3/";    
    AudioSource soundMaker;
    AudioClip attackClip;
    AudioClip hurtClip;
    AudioClip deathClip;
    public override void Start () {
        currentBufferDistance = 10f;
        base.Start();
        loot.Clear();
        loot.Add(new ItemSaveName("Dragon Hide", Random.Range(2, 5)));
        loot.Add(new ItemSaveName("Large Dragon Hide", 1));
        soundMaker = GetComponent<AudioSource>();
        // attackClip = Resources.Load(soundSource + "Goblin_01") as AudioClip;
        hurtClip = Resources.Load(soundSource + "Dragon_Growl_00") as AudioClip;
        deathClip = Resources.Load(soundSource + "Dragon_Growl_01") as AudioClip;
    }

	public override void FindEyes () {
		eyes = gameObject.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 Neck1/Bip001 Head/H").gameObject;
        // eyes = gameObject.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 Neck1/Bip001 Head").gameObject;   
    }

    private void EnableQuestRoseLoc()
    {
        Quest q = QuestManager.GetQuestByID("DragonQuest2");
        if(q.ID != null)
        {
            q.SetTarget("QuestRoseLoc", true);
        }
        MinimapIndicatorManager.UpdateMinimapIndicator();
    }

    private void EnableQuestThorinLoc()
    {
        Quest q = QuestManager.GetQuestByID("ThorinNecromancyQuest");
        if(q.ID != null)
        {
            q.SetTarget("QuestThorinsHouseLoc", true);
        }
        MinimapIndicatorManager.UpdateMinimapIndicator();
    }

    protected override void DoneDying()
    {
        EnemyManager.BossKilled(gameObject.name);
        base.DoneDying();
        // print("hi");
        if(GameManager.playerInventory.RemoveItem("Empty Cantis"))
        {
            print("swap out");
            GameManager.playerInventory.AddItem("Cantis Gem");
        }
    }

    protected override void CheckChunkDespawn()
    { 

    }

    // public override void CheckBeing()
    // {
    // 	switch (state.beingState)
    // 	{
    // 		case StateOfBeingEnum.attack:
    //             if (!attackScript.EnemiesAttack.GetComponent<Splash>().cooledDown)
    //             {
    //                 anime.SetTrigger("attack");
    //                 //GetComponent<EnemyAttackScript>().Attack();
    //                 attacking = true;
    //                 anime.SetBool("moving", false);
    //                 //print("attack");
    //                 //attackScript.Attack();
    //             }
    //             else
    //             { print("not cooled down"); }
    //             break;
    // 		case StateOfBeingEnum.move:
    //             // if (!attacking)
    //             // {
    //                 anime.SetBool("moving", true);
    //                 //Debug.Log("moving");
    //                 //STEFFEN this is where movement is being called~~~
    //                 Move();
    //             //}
    //             break;
    // 		default:
    // 			anime.SetBool("moving", false);
    // 			break;
    // 	}
    // }
    public void AttackFinished()
    {
        //attacking = false;
    }
    public void HitFinished()
    { 

    }
    public void HeroDie()
    {
        // print("asdf");
        if (GameManager.playerInventory.RemoveItem("Empty Cantis"))
        {
            // print("removed cantis");
            GameManager.playerInventory.AddItem("Cantis Gem");
            GameManager.DialogueBox.Variables["GotDragonEssence"] = "true";
            EnableQuestThorinLoc();
        }
        EnableQuestRoseLoc();
        GameManager.DialogueBox.Variables["DefeatedDragon"] = "true";
        base.DoneDying();
    }
    protected override void PlayHurtSound()
    {
        if (!GameManager.MuteSoundeEffects && !soundMaker.isPlaying)
        {
            soundMaker.clip = hurtClip;
            soundMaker.Play();
        }
    }
    protected override void PlayDeathSound()
    {
        // if (!GameManager.MuteSoundeEffects && !soundMaker.isPlaying)
        if (!GameManager.MuteSoundeEffects)
        {
            soundMaker.clip = deathClip;
            soundMaker.Play();
        }
    }
}
