using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageReachedObjective : StageObjective
{
    public override StageObjectiveType ObjectiveType => StageObjectiveType.Reach;
    
    public StageReachObject Objective => objective;

    [SerializeField] private StageReachObject objective;


    private void Awake()
    {
        objective.IsLockDetect = true;
    }

    public override void Enter()
    {
        objective.IsLockDetect = false;
    }

    public override string GetProgressText()
    {
        return "";
    }

    public override bool TryUpdateProgress<T>(T targret, out bool isComplete)
    {
        isComplete = true;
        return true;
    }
}


