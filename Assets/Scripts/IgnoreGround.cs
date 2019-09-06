using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Ignore();
    }

    void Awake(){
        Ignore();
    }

    void Ignore()
    { 
		// foreach (GameObject ground in GameObject.FindGameObjectsWithTag("Ground"))
        // {
        //     Collider2D c = ground.GetComponent<Collider2D>();
        //     if (GetComponent<MeshCollider>() != null)
        //     { 
		// 		Physics.Ignore2DCollision(c, GetComponent<MeshCollider>());
		// 	}
        //     else
        //     {
		// 		 Physics.IgnoreCollision(c, GetComponent<Collider>());
        //     }
        // }
	}
    // Update is called once per frame
    void Update () {
		
	}
}
