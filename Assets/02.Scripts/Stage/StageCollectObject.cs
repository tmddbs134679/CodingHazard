using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollectObject : StageObjectiveObject, IInteractable, IDetectable
{
    public override StageObjectiveType ObjectiveType => StageObjectiveType.Collect;
    
    public bool IsLockInteract { get; set; }
    public bool IsLockDetect { get; set; }

    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }
    
    public void Collect()
    {
        if (_outline != null)
        {
            _outline.enabled = false;
        }
        
        StageManager.Instance.UpdateObjectives(this);
    }


    public void Enter()
    {
        _outline.enabled = true;
    }

    public void Exit()
    {
        if (_outline != null)
        {
            _outline.enabled = false;
        }
    }
    


    public void Interact()
    {
        Collect();
    }
}
