using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTile : MonoBehaviour {
	public Transform Parent;

	public Vector2 GlobalPos
	{
		get
		{
			return new Vector2(transform.position.x, transform.position.z);
		}
	}

    public Vector2 LocalPos
    {
        get
        {
            return localPos;
        }
        set
        {
            localPos = value;
			transform.localPosition = new Vector3(localPos.x, 0, localPos.y);
        }
    }

    private Vector2 localPos;
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	public void Awake()
	{
		//Instantiate(Resources.Load("Brick"), new Vector3(newPos.x, newPos.y, -0.125f), Quaternion.identity) as GameObject;
        GetComponent<Renderer>().material = Resources.Load("Materials/dirt") as Material;
    }

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	public void Update()
	{
		// TileObj.transform.position = new Vector3(10, 0, 10);
		// print(GridPos);
	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		Destroy(gameObject);
	}
}