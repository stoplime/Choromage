using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Main Menu background
//https://stock-clip.com/video-footage/glow+in+the+dark+foggy/2

public class MainMenu : MonoBehaviour {

    GameObject LoadingScreen;

	// Use this for initialization
	void Start () {
		// Reset Dont destroy onload objects
        // List<string> objNames = new List<string>(new string[] {"GameManager", "SpellManager", "InventoryManager"});
        // GameObject obj;
        // for (int i = 0; i < objNames.Count; i++)
        // {
        //     obj = GameObject.Find(objNames[i]);
        //     if (obj != null)
        //     {
        //         Destroy(obj);
        //     }
        //     obj = null;
        // }
        // if()
	}
    void Awake()
    { 
        LoadingScreen = GameObject.Find("Loading Screen");
        if (LoadingScreen != null)
        {
            LoadingScreen.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

	public void NewGame ()
    {
        LoadingScreen.SetActive(true);
        GameManager.ResetNewGame();
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    { 
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene(1);
	}
    
    public void Save()
    {
        GameManager.SaveGame();
    }

    public void OptionsScreen()
    { 
		//show options screen
	}

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        if (LoadingScreen != null)
        {
            LoadingScreen.SetActive(true);
        }
        SceneManager.LoadScene(0);
    }

    public void ReloadScene()
    {
        if (LoadingScreen != null)
        {
            LoadingScreen.SetActive(true);
        }
        // SceneManager.LoadScene(0);
		SceneManager.LoadScene(1);
		GameManager.ToggleTime();
	}

    public void DeadToMainMenu()
    {
		SceneManager.LoadScene(0);
	}
}
