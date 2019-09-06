using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats: Stats {

	public int id;
	public string enemyName;
	//public string mindState;
	public string beingState;

	//TODO: implement different attacks for different states
	public float attack;

	/// <summary>
	/// The distance the enemy wants to keep between it and the player. Example strings could be: meleeDistance, fleeDistance, personalSpace, RangedDistance, splashDistance
	/// </summary>
	/// <typeparam name="string"></typeparam>
	/// <typeparam name="float"></typeparam>
	/// <returns></returns>
	protected Dictionary<string, float> bufferDistances = new Dictionary<string, float>();
	
	/// <summary>
	/// if there is a desired default.
	/// </summary>
	string defaultBufferName;

	protected Dictionary<string, float> sightDistances = new Dictionary<string, float>();

	protected Dictionary<string, float> moveSpeeds = new Dictionary<string, float>();

	//protected List<Dictionary<string, float>> 

    //public float [] attack;
    //public float [] defense;


    //~~~~~~end json stuff
    TextMesh healthText;

    public override void Start () {
		maxHealth = 25;
        base.Start();
        healthText = GetComponentInChildren<TextMesh>();
    }

    public virtual void Update()
    {
        HealthTextUpdater();
		
    }
    void HealthTextUpdater()
    {
        if (healthText != null)
        {
            //healthText.transform.rotation = Quaternion.identity;
            healthText.text = ((int)health).ToString();
        }
        else
        { 
			healthText = GetComponentInChildren<TextMesh>();
		}
    }

	public float GetSpeed(string state)
	{
		if (moveSpeeds.ContainsKey(state))
		{
			return moveSpeeds[state];
		}
		return 3f;
	}

	public float GetSight(string state)
	{
		if (sightDistances.ContainsKey(state))
		{
			return sightDistances[state];
		}
		return 15f;
	}

	public float GetBuffer(string state)
	{
		if (bufferDistances.ContainsKey(state))
		{
			return bufferDistances[state];
		}
        // foreach (KeyValuePair <string, float> k in bufferDistances)
        // {
        //     print("KEY:" +k.Key);
        // }
        //print(bufferDistances.Count);
        //print ("no " + state);
        return 10f;
	}
}
