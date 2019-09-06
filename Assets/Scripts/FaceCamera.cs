using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Intended to attatch to textmeshes, makes sure object always faces camera
/// </summary>
public class FaceCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// GetComponent<MeshRenderer>().sortingLayerName = "NameTags";
		GetComponent<MeshRenderer>().sortingOrder = 300;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Camera.main.transform.rotation;
	}
}
