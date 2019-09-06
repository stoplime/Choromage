using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicTransition : MonoBehaviour {

    AudioSource forestMusic;
    AudioSource cityMusic;
    GameObject player;
    bool doOnceForest;
    bool doOnceCity;
    public AudioClip forestClip;
    public AudioClip cityClip;
    AudioClip next;
    AudioSource musicPlayer;
    //bool forestMusicPlaying = true;
    bool inForest;
    bool fadingOut;
    // bool fadingIn;
    float currentVolume;
    float fadeSpeed =.05f;
    public static MusicTransition musicInstance;
    public float maxForestVolume = .5f;

    // Use this for initialization
    void Awake () {
        //forestMusic.Play();
        DontDestroyOnLoad(gameObject);
        if (musicInstance == null) {
            musicInstance = this;
        } else {
            DestroyObject(gameObject);
        }
        musicPlayer = GetComponent<AudioSource>();
        inForest = false;
        currentVolume = musicPlayer.volume;
        // forestMusic = GameObject.Find ("Forest Music").GetComponent<AudioSource>();
        // cityMusic = GameObject.Find("City Music").GetComponent<AudioSource>();
        // player = GameObject.Find("Player");
        // forestClip = Resources.Load("Sounds and Music/")
        // doOnceForest = true;
        // doOnceCity = true;

    }

    // Update is called once per frame
    void Update () {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (GameManager.PlayerPos.x >= 1090 && GameManager.PlayerPos.z > -1300)
            {
                if (!inForest)
                {
                    // musicPlayer.clip = forestClip;
                    // musicPlayer.Play();
                    inForest = true;
                    FadeSwitch(forestClip);
                }
                // if (doOnceForest == true)
                // {
                //     // cityMusic.Stop();
                //     // forestMusic.Play();

                //     doOnceForest = false;
                //     doOnceCity = true;
                // }
            }
            else if (GameManager.PlayerPos.x < 1090 && GameManager.PlayerPos.z > -1300)
            {
                if (inForest)
                {
                    inForest = false;
                    FadeSwitch(cityClip);
                    // musicPlayer.clip = cityClip;
                    // musicPlayer.Play();
                }
                // if (doOnceCity == true)
                // {
                //     forestMusic.Stop();
                //     cityMusic.Play();
                //     doOnceForest = true;
                //     doOnceCity = false;
                // }
            }
            else //if (GameManager.player.transform.position.x < 1090 && GameManager.player.transform.position.z > -1300)
            { 
                if (!inForest)
                {
                    // musicPlayer.clip = forestClip;
                    // musicPlayer.Play();
                    inForest = true;
                    FadeSwitch(forestClip);
                }
            }
        }
        Fading();
    }

    void FadeSwitch(AudioClip toPlay)
    {
        next = toPlay;
        fadingOut = true;
    }

    void Fading()
    {
        if (fadingOut)
        {
            if (musicPlayer.volume > 0)
            {
                if (musicPlayer.volume / 10 > fadeSpeed)
                {
                    musicPlayer.volume -= Time.unscaledDeltaTime * musicPlayer.volume / 10;
                }
                else
                {
                    musicPlayer.volume -= Time.unscaledDeltaTime * fadeSpeed;
                }
            }
            else 
            {
                fadingOut = false;
                musicPlayer.clip = next;
                musicPlayer.Play();
            }
        }
        else if (musicPlayer.volume < currentVolume && !inForest)
        { 
            if (1-musicPlayer.volume / 10 < fadeSpeed)
            {
                musicPlayer.volume += Time.unscaledDeltaTime * musicPlayer.volume / 10;
            }
            else
            {
                musicPlayer.volume += Time.unscaledDeltaTime * fadeSpeed;
            }
            // musicPlayer.volume += Time.unscaledDeltaTime * fadeSpeed;
        }
        else if (musicPlayer.volume < currentVolume *maxForestVolume && inForest)
        { 
            if (1-musicPlayer.volume / 10 < fadeSpeed)
            {
                musicPlayer.volume += Time.unscaledDeltaTime * musicPlayer.volume / 10;
            }
            else
            {
                musicPlayer.volume += Time.unscaledDeltaTime * fadeSpeed;
            }
            // musicPlayer.volume += Time.unscaledDeltaTime * fadeSpeed;
        }
        else if (musicPlayer.volume != currentVolume & !inForest)
        {
            musicPlayer.volume = currentVolume;
        }
        else if (musicPlayer.volume != currentVolume *maxForestVolume && inForest)
        {
            musicPlayer.volume = currentVolume *maxForestVolume;
        }
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            forestMusicPlaying = !forestMusicPlaying;
            if (forestMusicPlaying == true)
            {
                cityMusic.Stop();
                forestMusic.Play();
            }

            if (forestMusicPlaying == false)
            {
                forestMusic.Stop();
                cityMusic.Play();
            }
        }
    }*/

}
