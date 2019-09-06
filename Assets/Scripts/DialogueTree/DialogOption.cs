using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogOption : MonoBehaviour {

    string keyVal;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Chosen);
    }

    /// <summary>
    /// The Id of the selected dialogue option
    /// </summary>
    /// <value></value>
    public string KeyVal
    {
        get {
            return keyVal;
        }
        set {
            keyVal = value;
        }
    }

    public void Chosen()
    {
        // Debug.Log("chosen\t"+ keyVal.ToString());
        transform.parent.parent.GetComponent<DialogBox>().ChoseOption(keyVal);
    }

}
