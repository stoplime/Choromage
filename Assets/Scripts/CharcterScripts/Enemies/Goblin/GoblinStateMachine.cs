using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStateMachine :EnemyStateMachine {

    // Use this for initialization
    List<string> possibleEquipmentDrops;
    string soundSource ="Sounds/Fantasy Sfx/Mp3/";
    AudioSource soundMaker;
    AudioClip attackClip;
    AudioClip hurtClip;
    AudioClip deathClip;
    public override void Start () {

        currentBufferDistance = 7.5f;
        base.Start();
        loot.Clear();
        loot.Add(new ItemSaveName("Rock", Random.Range(5, 20)));
        // loot.Add(new ItemSaveName("Brick", Random.Range(0, 2)));
        // loot.Add(new ItemSaveName("Jewelry", Random.Range(0,100) > 90 ? 1 : 0));
        // loot.Add(new ItemSaveName(RandomEquipment(), Random.Range(0,100) > 90 ? 1 : 0));
        loot.Add(new ItemSaveName(RandomEquipment(), Random.Range(1, 2)));
        soundMaker = GetComponent<AudioSource>();
        attackClip = Resources.Load(soundSource + "Goblin_01") as AudioClip;
        hurtClip = Resources.Load(soundSource + "Goblin_03") as AudioClip;
        deathClip = Resources.Load(soundSource + "Goblin_04") as AudioClip;
        //        print (base.currentBufferDistance);
    }

	public override void FindEyes () {
		eyes = gameObject.transform.Find("Armature/Bones/Hips_001/Hips/Spine/Chest/Neck/Head/Eyes").gameObject;   
    }
    string RandomEquipment()
    {
        possibleEquipmentDrops = new List<string>();
        possibleEquipmentDrops.Add("Peasants Shirt");
        possibleEquipmentDrops.Add("Peasants Leggings");
        possibleEquipmentDrops.Add("Peasants Hood");
        return possibleEquipmentDrops[Random.Range(0, possibleEquipmentDrops.Count)];
    }
    protected override void PlayAttackSound()
    {
        if (!GameManager.MuteSoundeEffects && !soundMaker.isPlaying)
        {
            // if (Random.Range(0, 2) >= 1f)
            // {
            //     soundMaker.clip = attackClip;
            //     soundMaker.Play();
            // }
        }
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
    // // Update is called once per frame
    // void Update () {

    // }
}
