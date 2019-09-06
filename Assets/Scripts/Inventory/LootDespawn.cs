using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDespawn : MonoBehaviour {
	private Inventory inventory;

	private void Start()
	{
		inventory = GetComponent<Inventory>();
	}

	private void Update()
	{
		if (inventory.IsEmpty && inventory.HasInitialized && !TopMenu.isOpen)
		{
			Destroy(gameObject);
		}
        else
        {
            if (gameObject.name == "LootBag")
            {
                GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }
    }
}
