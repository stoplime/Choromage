using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HoveringOverSpell(GameObject hotbarButton)
    {
        if (GetComponentInChildren<SpellData>() != null)
        {
            //print("hover");
            GetComponentInChildren<SpellData>().OnSpell(hotbarButton);
        }
    }
    public void LeftSpell(GameObject hotbarButton)
    { 
        if (GetComponentInChildren<SpellData>() != null)
        {
            //print("hover");
            GetComponentInChildren<SpellData>().LeftSpell();
        }
    }
}
