using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFade : MonoBehaviour {

    public bool NoFade = false;
    public static float fadeAmount = 0.5f;
    float alphaNum;
    public string resourcePathway;
    float maxFade;

    void Start()
    {
        alphaNum = 1.0f;
        maxFade = 1.0f - fadeAmount;
        gameObject.layer = 15;
    }

    bool CheckObjectBehind(Collider other)
    {
        return other.gameObject.CompareTag("Player")    || 
               other.gameObject.CompareTag("Enemy")     || 
               other.gameObject.CompareTag("Inventory");
    }

    void OnTriggerStay(Collider other)
    {
        if (!NoFade)
        {
            gameObject.layer = 9;
            if (CheckObjectBehind(other))
            {
                if (alphaNum > maxFade)
                {
                    alphaNum -= fadeAmount;
                }
            }
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(1, 1, 1, alphaNum);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!NoFade)
        {
            gameObject.layer = 15;
            if (CheckObjectBehind(other))
            {
                if (alphaNum < 1.0f)
                {
                    alphaNum += fadeAmount;
                }
            }
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(1, 1, 1, alphaNum);
            }
        }
    }

}
