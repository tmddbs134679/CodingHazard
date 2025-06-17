using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageReachObjective : StageObjective
{
    public override StageObjectiveType ObjectiveType => StageObjectiveType.Reach;

    [Space(20f)]
    [SerializeField] private StageReachObject objective;


    public override void Enter()
    {
    }

    public override StageObjectiveObject GetTargetObjectiveObject()
    {
        return objective;
    }
    

    public override string GetProgressText()
    {
        return "";
    }

    public override void UpdateProgress<T>(T target, out bool isClear)
    {
        isClear = true;
    }
}


