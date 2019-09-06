using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonDescription : MonoBehaviour
{
    private EventTrigger textEventTrigger;
    // public GameObject skilldescEmptyBox;
    GameObject skilldescEmptyBox;
    Text tipDescription;
    float timer;
    bool closed = true;

    void Awake()
    {
        // print(gameObject);
        Transform [] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.name == "PopupTextHolder")
            { 
                skilldescEmptyBox = child.gameObject;
            }
        }
        // print(gameObject);
        skilldescEmptyBox.SetActive(false);
        textEventTrigger = GetComponent<EventTrigger>();
        tipDescription = skilldescEmptyBox.GetComponentInChildren<Text>();
        if (textEventTrigger != null)
        {
            textEventTrigger.triggers[0].callback.AddListener(delegate { MouseEnter(); });
            textEventTrigger.triggers[1].callback.AddListener(delegate { MouseExit(); });
        }

    }

    void Update()
    {
        if (closed == false)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > 1)
            {
                skilldescEmptyBox.SetActive(true);
                timer = 0f;
            }
        }
    }

    void MouseEnter()
    {
        FindText();
        /*timer += Time.deltaTime;
        if (timer > 3)
        {
            skilldescEmptyBox.SetActive(true);
            timer = 0f;
        }*/
        //skilldescEmptyBox.SetActive(true);
        if (gameObject.tag != "Spell Button")
        {
            closed = false;
        }
        else 
        { 
            switch (gameObject.name)
                {
                    case "Spell1Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(1))
                        {
                            closed = false;
                        }
                        break;
                    case "Spell2Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(2))
                        {
                            closed = false;
                        }
                        break;
                    case "Spell3Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(3))
                        {
                            closed = false;
                        }
                        break;
                    case "Spell4Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(4))
                        {
                            closed = false;
                        }
                        break;
                    case "Spell5Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(5))
                        {
                            closed = false;
                        }
                        break;
                    case "Spell6Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(6))
                        {
                            closed = false;
                        }
                        break;
                    case "SpellDragPrefab(Clone)":
                        closed = false;
                        break;
            }
        }
    }

    void MouseExit()
    {
        skilldescEmptyBox.SetActive(false);
        closed = true;
    }
    void FindText()
    {
        string newText = "";
        if (gameObject.tag == "Spell Button")
        {
            if (gameObject.GetComponent<SpellData>() != null)
            { 
                newText = gameObject.GetComponent<SpellData>().PopUpSpell.ToolTips();
            }
            else
            {
                switch (gameObject.name)
                {
                    case "Spell1Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(1))
                        {
                            newText = GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells[1].ToolTips();
                        }
                        break;
                    case "Spell2Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(2))
                        {
                            newText = GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells[2].ToolTips();
                        }
                        break;
                    case "Spell3Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(3))
                        {
                            newText = GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells[3].ToolTips();
                        }
                        break;
                    case "Spell4Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(4))
                        {
                            newText = GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells[4].ToolTips();
                        }
                        break;
                    case "Spell5Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(5))
                        {
                            newText = GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells[5].ToolTips();
                        }
                        break;
                    case "Spell6Button":
                        if(GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells.ContainsKey(6))
                        {
                            newText = GameManager.Cantis.GetComponent<PlayerSpells>().HotSpells[6].ToolTips();
                        }
                        break;
                }
            }
        }
        // else if (gameObject.tag == "Item Button")
        // { 

        // }
        if (newText != "")
        {
            tipDescription.text = newText;
        }
    }

}