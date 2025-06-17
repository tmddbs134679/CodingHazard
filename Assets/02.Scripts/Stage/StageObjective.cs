using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class StageObjective : MonoBehaviour
{
    public abstract StageObjectiveType ObjectiveType { get; }

    [field: SerializeField] public string Description { get; private set; }


    protected StageManager StageManager { get; set; }

    public virtual void Init(StageManager stageManager)
    {
        StageManager = stageManager;
    }
    
    public abstract StageObjectiveObject GetTargetObjectiveObject();
    public abstract void Enter();
    public abstract string GetProgressText();
    public abstract void UpdateProgress<T>(T target, out bool isClear) where T : StageObjectiveObject;

}
