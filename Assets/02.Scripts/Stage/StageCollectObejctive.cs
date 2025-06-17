using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollectObejctive : StageObjective
{
    public override StageObjectiveType ObjectiveType => StageObjectiveType.Collect;
    public List<StageCollectObject> ObjectiveItems => objectiveItems;
    
    
    [Space(10f)]
    [SerializeField] private List<StageCollectObject> objectiveItems;
    
    
    private int _maxCount;
    private int _curCount;
    
    

    private void Awake()
    {
        _maxCount = objectiveItems.Count;

        foreach (var item in objectiveItems)
        {
            item.IsLockInteract = true;
        }
    }
    


    public override void Enter()
    {
        foreach (var item in objectiveItems)
        {
            item.IsLockInteract = false;
            item.ToggleOutline(true);
        }
    }
    
    

    public override string GetProgressText()
    {
        return $"( {_curCount} / {_maxCount} )";
    }
    
    

    public override bool TryUpdateProgress<T>(T targret, out bool isComplete) 
    {
        if (targret is StageCollectObject item)
        {
            if (objectiveItems.Contains(item))
            {
                objectiveItems.Remove(item);
                
                Destroy(item.gameObject);
            
                _curCount += 1;

                isComplete = _curCount >= _maxCount;
            
                if (_curCount >= _maxCount)
                {
                    onCompleteEvent?.Invoke();
                }
                
                return true;
            }
        }

        isComplete = false;
        return false;
    }
    
}
