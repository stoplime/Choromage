using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfStateMachine : EnemyStateMachine {

    // Use this for initialization
    string soundSource ="Sounds/Wolf/";    
    AudioSource soundMaker;
    AudioClip attackClip;
    AudioClip aggressiveClip;
    AudioClip hurtClip;
    AudioClip deathClip;
    public override void FindEyes () {
        eyes = gameObject.transform.Find("Bip02/Bip02 Pelvis/Bip02 Spine/Bip02 Spine1/Bip02 Spine2/Bip02 Neck/Bip02 Neck1/Bip02 Head/Eyes").gameObject;
    }

    public override void Start () {
        base.Start();
        loot.Clear();
        loot.Add(new ItemSaveName("Fur", Random.Range(1, 3)));        
        loot.Add(new ItemSaveName("Meat", Random.Range(2, 4)));
        if (GameManager.GodMode)
        { 
            loot.Add(new ItemSaveName("Fur", 5));        
            loot.Add(new ItemSaveName("Meat", 10));
        }
        if (GetComponent<TutorialWolfStats>()!=null)
        {
            enemysStats = GetComponent<TutorialWolfStats>();
        }
        else
        {
            enemysStats = GetComponent<WolfStats>();
        }
                soundMaker = GetComponent<AudioSource>();
        attackClip = Resources.Load(soundSource + "Bite-SoundBible.com-2056759375") as AudioClip;
        aggressiveClip = Resources.Load(soundSource + "Wolf Growling Fiercely-SoundBible.com-667953206") as AudioClip;
        // hurtClip = Resources.Load(soundSource + "Goblin_03") as AudioClip;
        deathClip = Resources.Load(soundSource + "Coyote Call-SoundBible.com-1347099109") as AudioClip;
    }
    // // Update is called once per frame
    // void Update () {

    // }
    protected override void PlayAttackSound()
    {
        // if (!GameManager.MuteSoundeEffects && !soundMaker.isPlaying)
        // {
        //     // if (Random.Range(0, 2) >= 1f)
        //     // {
        //         soundMaker.clip = attackClip;
        //         soundMaker.Play();
        //     // }
        // }
    }
    
    protected override void PlayHurtSound()
    {
        // if (!GameManager.MuteSoundeEffects && !soundMaker.isPlaying)
        // {
        //     soundMaker.clip = hurtClip;
        //     soundMaker.Play();
        // }
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
    protected override void PlayAggressiveSound()
    {
        // if (!GameManager.MuteSoundeEffects && !soundMaker.isPlaying)
        // print("be");
        if (!GameManager.MuteSoundeEffects&& !soundMaker.isPlaying)
        {
            soundMaker.clip = aggressiveClip;
            print(aggressiveClip);
            soundMaker.Play();
        }
    }
}
