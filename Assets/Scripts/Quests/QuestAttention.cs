using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestAttention {
	/// <summary>
	/// List of all the particles that are currently attached to an NPC
	/// </summary>
    public static Dictionary<string, GameObject> AttachedParticles = new Dictionary<string, GameObject>();

    private static GameObject particlePrefb;

    public static void Initialize()
    {
        particlePrefb = Resources.Load<GameObject>("Characters/TargetCircles/Particles");
    }

    public static void AttachParticle(string NPC)
    {
        GameObject NPCObj = GameObject.Find("NPCs").transform.Find(NPC).gameObject;
        AttachParticle(NPCObj);
    }

    public static void AttachParticle(GameObject NPC)
    {
        if (!AttachedParticles.ContainsKey(NPC.name))
        {
            AttachedParticles.Add(NPC.name, MonoBehaviour.Instantiate(particlePrefb, NPC.transform));
        }
    }

    public static void RemoveParticle(GameObject NPC)
    {
        RemoveParticle(NPC.name);
    }

    public static void RemoveParticle(string NPC)
    {
        if (AttachedParticles.ContainsKey(NPC))
        {
            MonoBehaviour.Destroy(AttachedParticles[NPC]);
            AttachedParticles.Remove(NPC);
        }
    }

}
