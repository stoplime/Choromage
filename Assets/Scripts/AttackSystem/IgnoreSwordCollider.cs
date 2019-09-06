using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreSwordCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GetComponent<MeshCollider>() != null)
        {
            Physics.IgnoreCollision(GameObject.Find("Sword").GetComponent<Collider>(), GetComponent<MeshCollider>());
        }
        else
        {
            Physics.IgnoreCollision(GameObject.Find("Sword").GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
