using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOverlayController : MonoBehaviour {
	public GameObject PauseOverlay { get; set; }

	// Use this for initialization
	void Start () {
		PauseOverlay = GameObject.Find("PauseOverlay");
	}
	
	// Update is called once per frame
	void Update () {
        if (PauseOverlay != null)	//TO DO:  better fix for main menu
        {
            if (GameManager.IsPaused)
            {
                PauseOverlay.SetActive(true);
            }
            else
            {
                PauseOverlay.SetActive(false);
            }
        }
    }
}
