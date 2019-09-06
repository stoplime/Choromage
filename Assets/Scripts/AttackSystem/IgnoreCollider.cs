using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ignores colliders of children and has the method Ignore which can be called to ignore anything sharing the same name as the string passed. MeshColliders have been taken into account so collisions will be ignored no matter what it is. 
/// </summary>
public class IgnoreCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GetComponent<Collider>() != null)
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(c, GetComponent<Collider>());
            }
            foreach (MeshCollider m in GetComponentsInChildren<MeshCollider>())
            {
                Physics.IgnoreCollision(m, GetComponent<Collider>());
            }
        }
        else
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(c, GetComponent<MeshCollider>());
            }
            foreach (MeshCollider m in GetComponentsInChildren<MeshCollider>())
            {
                Physics.IgnoreCollision(m, GetComponent<MeshCollider>());
            }
        }
	}

    /// <summary>
    /// Will ignore anything with the same name as the string passed. Can attach to all types of (collidable) gameobjects.
    /// </summary>
    /// <param name="ignoring"></param>
    public void Ignore(string ignoring)
    {
        if (GetComponent<MeshCollider>() != null)
        {
            foreach (Collider c in GameObject.Find(ignoring).GetComponents<Collider>())
            {
                Physics.IgnoreCollision(c, GetComponent<MeshCollider>());
            }
            foreach (MeshCollider m in GameObject.Find(ignoring).GetComponents<MeshCollider>())
            {
                Physics.IgnoreCollision(m, GetComponent<MeshCollider>());
            }
        }
        else
        {
            foreach (Collider c in GameObject.Find(ignoring).GetComponents<Collider>())
            {
                Physics.IgnoreCollision(c, GetComponent<Collider>());
            }
            foreach (MeshCollider m in GameObject.Find(ignoring).GetComponents<MeshCollider>())
            {
                Physics.IgnoreCollision(m, GetComponent<Collider>());
            }
        }
    }


	// Update is called once per frame
	void Update () {
		
	}
}
