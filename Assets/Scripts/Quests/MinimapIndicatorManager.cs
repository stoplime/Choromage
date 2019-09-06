using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinimapIndicatorManager
{
    /// <summary>
    /// Temporaraly enables or disables all the arrows (used for being indoors)
    /// </summary>
    public static bool enable = true;

    /// <summary>
    /// List of the green arrows
    /// </summary>
    public static List<GameObject> MapTargets;
    /// <summary>
    /// List of the actual target locations
    /// </summary>
    public static List<GameObject> Targets;

    /// <summary>
    /// Prefab of the green arrows
    /// </summary>
    public static GameObject MapIndicatorObj;
    /// <summary>
    /// Location of the parent of the green arrows
    /// </summary>
    public static GameObject MapIndicatorParent;
    
    public static void Initialize()
    {
        MapIndicatorObj = Resources.Load<GameObject>("UI Stuff/MiniMap/Minimap Indicator Target");
        MapIndicatorParent = GameObject.Find("MiniMapIndicatorParent");
        if (MapIndicatorParent == null)
        {
            Help.print("MiniMapIndicatorParent GameObject Not Found");
        }
        MapTargets = new List<GameObject>();
        Targets = new List<GameObject>();
    }

    private static GameObject CreateNewMapTargetIndicator(GameObject target)
    {
        GameObject newMapTargetIndicator = MonoBehaviour.Instantiate(MapIndicatorObj, MapIndicatorParent.transform);
        newMapTargetIndicator.GetComponent<MinimapIndicatorController>().Target = target;
        return newMapTargetIndicator;
    }

    private static void RemoveMapTargets(List<GameObject> targets)
    {
        for (int i = MapTargets.Count-1; i >= 0; i--)
        {
            MinimapIndicatorController controller = MapTargets[i].GetComponent<MinimapIndicatorController>();
            if (targets.Contains(controller.Target))
            {
                MonoBehaviour.Destroy(MapTargets[i]);
                MapTargets.RemoveAt(i);
            }
        }
    }

    public static void UpdateMinimapIndicator()
    {
        List<GameObject> oldTargets = new List<GameObject>(Targets);
        Targets.Clear();
        foreach (Quest quest in QuestManager.ListOfQuests)
        {
            List<GameObject> qtargets = quest.GetTargets();
            for (int i = 0; i < qtargets.Count; i++)
            {
                if (!oldTargets.Contains(qtargets[i]) && enable)
                {
                    // Add new Target
                    MapTargets.Add(CreateNewMapTargetIndicator(qtargets[i]));
                }
                // Adds the target to the list regardless of whether oldTarget has it
                Targets.Add(qtargets[i]);
            }
        }
        // test(Targets, oldTargets);
        for (int i = oldTargets.Count-1; i >= 0; i--)
        {
            if (Targets.Contains(oldTargets[i]))
            {
                oldTargets.RemoveAt(i);
            }
        }
        RemoveMapTargets(oldTargets);
    }

    public static void Disable()
    {
        if (enable)
        {
            enable = false;
            // removes all the targets
            RemoveMapTargets(Targets);
        }
    }

    public static void Enable()
    {
        if (!enable)
        {
            enable = true;
            foreach (GameObject target in Targets)
            {
                MapTargets.Add(CreateNewMapTargetIndicator(target));
            }
        }
    }

    public static void ClearAllTargets()
    {
        RemoveMapTargets(Targets);
        Targets.Clear();
        foreach (Quest quest in QuestManager.ListOfQuests)
        {
            List<string> qtargets = quest.GetPossibleTargets();
            for (int i = 0; i < qtargets.Count; i++)
            {
                quest.SetTarget(qtargets[i], false);
            }
        }
    }

    // private static void test(List<GameObject> current, List<GameObject> old)
    // {
    //     bool in_old = false;
    //     bool in_current = false;
    //     foreach (var item in current)
    //     {
    //         if (item.name == "QuestForestExitLoc")
    //         {
    //             in_current = true;
    //         }
    //     }
    //     foreach (var item in old)
    //     {
    //         if (item.name == "QuestForestExitLoc")
    //         {
    //             in_old = true;
    //         }
    //     }
    //     if (in_current && !in_old)
    //     {
    //         Help.print("Added QuestForestExitLoc");
    //     }
    // }
}
