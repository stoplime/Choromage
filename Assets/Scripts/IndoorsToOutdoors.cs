using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorsToOutdoors : MonoBehaviour
{
    GameObject myParent;
    // Start is called before the first frame update
    void Start()
    {
        myParent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown("q"))
        {
            myParent.GetComponent<DoorOutline>().indoorsPressedQ = true;
        }
    }

}
