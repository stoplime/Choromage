using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats {

	int experience;
	int nextLvlExp = 100;
    Vector3 lastSafePoint;
    public float MaxMana
	{
        get { return TotalMaxMana; }
    }
	public float MaxHealth
	{
        get { return TotalMaxHealth; }
    }
	public int Experience
    {
        get { return experience; }
    }

	public int NextLevel
    { 
		get {return  nextLvlExp; }
    }
    // Use this for initialization

    TextMesh healthText;
    GameObject deathScreen;
    
    public float BaseMaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public float BaseMaxMana
    {
        get { return maxMana; }
        set { maxMana = value; }
    }
    public float BaseManaRegen
    {
        get { return manaRegen; }
        set { manaRegen = value; }
    }
    public float BaseHealthRegen
    {
        get { return healthRegen; }
        set { healthRegen = value; }
    }
    // public int Gold
    // {
    //     get { return gold; }
    //     set { gold = value; }
    // }
    public override void Start () {
        maxHealth = 50;
        maxMana = 30;
        healthRegen = TotalMaxHealth/10;
        manaRegen = TotalMaxMana/5;
        lastSafePoint = transform.position;
        base.Start();
		healthText = GetComponentInChildren<TextMesh>();
        //castSpeed = -1f;
        // deathScreen = GameObject.Find("DeadScreenPanel");
        // deathScreen.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void FullHeal()
    {
        health = maxHealth;
        gameObject.GetComponent<PlayerController>().Recovered();
    }

    public void Respawn()
    {
        transform.position = lastSafePoint;
        FullHeal();
    }
}
