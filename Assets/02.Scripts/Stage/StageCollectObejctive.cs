using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollectObejctive : StageObjective
{
    public override StageObjectiveType ObjectiveType => StageObjectiveType.Collect;
    
    
    [Space(20f)]
    [SerializeField] private List<StageCollectObject> objectiveItems;
    
    
    private int _maxCount;
    private int _curCount;


   
    public override void Init(StageManager stageManager)
    {
        base.Init(stageManager);
        
        _maxCount = objectiveItems.Count;

        foreach (var item in objectiveItems)
        {
            item.IsLockInteract = true;
            item.IsLockDetect = true;
        }
    }

    public override void Enter()
    {
        var item = GetTargetObjectiveObject() as StageCollectObject;

        item.IsLockDetect = false;
        item.IsLockInteract = false;
    }
    
    
    public override StageObjectiveObject GetTargetObjectiveObject()
    {
        if (objectiveItems.Count > 0)
        {
            return objectiveItems[0];
        }

        return null;
    }


    public override string GetProgressText()
    {
        return $"( {_curCount} / {_maxCount} )";
    }
    

    public override void UpdateProgress<T>(T target, out bool isClear)
    {
        isClear = false;

        if (target == GetTargetObjectiveObject())
        {
            Destroy(target.gameObject);

            objectiveItems.RemoveAt(0);
            
            _curCount += 1;

            if (_curCount >= _maxCount)
            {
                isClear = true;
            }
            else
            {
                var item = GetTargetObjectiveObject() as StageCollectObject;

                item.IsLockDetect = false;
                item.IsLockInteract = false;
            }
        }
    }
}
