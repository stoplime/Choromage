using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveOnce : MonoBehaviour
{
    Button SaveBt;
    // Start is called before the first frame update
    void Start()
    {
        SaveBt = GetComponent<Button>();
    }

    void OnDisable()
    {
        SaveBt.interactable = true;
    }

    public void PressedOnce()
    {
        SaveBt.interactable = false;
    }
}
