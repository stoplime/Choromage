using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade : MonoBehaviour {

	//TODO: Check 
    int itemPrice;
    int itemValue;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Buy()
    {
        if (GameManager.player.GetComponent<Stats>().Gold >= itemPrice)
        {
            GameManager.player.GetComponent<Stats>().Gold -= itemPrice;
        }
    }

    void Sell()
    {
        GameManager.player.GetComponent<Stats>().Gold += itemValue;
    }
}
