using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDragonStateMachine :EnemyStateMachine {

    string soundSource ="Sounds/Dragon/";    
    AudioSource soundMaker;
    AudioClip attackClip;
    AudioClip hurtClip;
    AudioClip deathClip;
    public override void Start () {
        currentBufferDistance = 10f;
        base.Start();
        loot.Clear();
        loot.Add(new ItemSaveName("Dragon Hide", Random.Range(1, 3)));
        if (GameManager.GodMode)
        { 
            loot.Add(new ItemSaveName("Dragon Hide", 5));        
        }
        soundMaker = GetComponent<AudioSource>();
        attackClip = Resources.Load(soundSource + "Dragon Bite-SoundBible.com-1625781385") as AudioClip;
        deathClip = Resources.Load(soundSource + "Velociraptor Call-SoundBible.com-1782075819") as AudioClip;
        //loot.Add(new ItemSaveName("Dragon Hide", Random.Range(0, 3)));        
    }

	public override void FindEyes () {
		eyes = gameObject.transform.Find("B_Root/B_bip01/B_00/B_01/B_02/B_03/Eyes").gameObject;
        
    }
    protected override void PlayAttackSound()
    {
        if (!GameManager.MuteSoundeEffects && !soundMaker.isPlaying)
        {
            soundMaker.clip = attackClip;
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
    // // Update is called once per frame
    // void Update () {

    // }
}
