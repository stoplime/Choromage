using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverSign : MonoBehaviour
{
    GameObject currentParent;
    GameObject currentSign;
    void Start()
    {
        currentSign = GameObject.Find("SignDefault");
        currentParent = GameObject.Find("ParentDefault");
    }

    // Update is called once per frame
    Ray ray;
    RaycastHit hit;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Sign")
            {
                currentParent = hit.collider.gameObject.transform.parent.gameObject;
                if (currentParent!=null)
                {
                    foreach (Transform child in currentParent.transform)
                    {
                        if (child.name == "SignPopup1")
                        {
                            currentSign = currentParent.transform.GetChild(2).gameObject;
                            currentSign.gameObject.SetActive(true);
                        }
                        if (child.name == "SignPopup2")
                        {
                            currentSign = currentParent.transform.GetChild(3).gameObject;
                            currentSign.gameObject.SetActive(true);
                        }
                        if (child.name == "SignPopup3")
                        {
                            currentSign = currentParent.transform.GetChild(4).gameObject;
                            currentSign.gameObject.SetActive(true);
                        }
                    }
                }
            }

            else {
                if (currentParent!=null)
                {
                    foreach (Transform child in currentParent.transform)
                    {
                        if (child.name == "SignPopup1")
                        {
                            currentSign = currentParent.transform.GetChild(2).gameObject;
                            currentSign.gameObject.SetActive(false);
                        }
                        if (child.name == "SignPopup2")
                        {
                            currentSign = currentParent.transform.GetChild(3).gameObject;
                            currentSign.gameObject.SetActive(false);
                        }
                        if (child.name == "SignPopup3")
                        {
                            currentSign = currentParent.transform.GetChild(4).gameObject;
                            currentSign.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

    }

}
