using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIndicatorController : MonoBehaviour {
    public GameObject Target;

    private float Radius = 41;
    private float Offset = 8;

    void Update()
    {
        if (Target != null)
        {
            Vector3 player2targetV3 = Target.transform.position - GameManager.PlayerPos;
            player2targetV3.y = 0;
            float dist = player2targetV3.magnitude - Offset;
            player2targetV3 = player2targetV3.normalized;

            if (dist > Radius)
            {
                dist = Radius;
            }

            transform.position = player2targetV3 * dist + GameManager.PlayerPos;
            transform.rotation = Quaternion.LookRotation(player2targetV3, new Vector3(0, 1, 0));
        }
    }
}

