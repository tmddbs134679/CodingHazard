using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollectObject : StageObjectiveObject, IInteractable
{
    public override StageObjectiveType ObjectiveType => StageObjectiveType.Collect;
    
    public bool IsLockInteract { get; set; }


    public void Collect()
    {
        ToggleOutline(false);
        
        StageManager.Instance.UpdateObjectives(this);
    }

    
    public void Enter()
    {
        ToggleOutline(true);
    }

    public void Exit()
    {
        ToggleOutline(false);
    }


    public void Interact()
    {
        Collect();
    }
}
