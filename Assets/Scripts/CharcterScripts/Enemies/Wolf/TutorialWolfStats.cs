using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWolfStats : WolfStats
{
    public override void Start()
    {
        base.Start();
    }
    public override void SetDistances()
    {
        base.SetDistances();	//sets buffer distance
        moveSpeeds.Add("aggressive", 10f);
        sightDistances.Add("aggressive", 100f);
        sightDistances.Add("guarded", 100f);
        sightDistances.Add("passive", 100);
    }

}
