using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageObjectiveObject : MonoBehaviour, IDetectable
{
    public abstract StageObjectiveType ObjectiveType { get; }
    
    public bool IsLockDetect { get; set; }

    
    private Outline _outline;
    


    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }
    
    public void ToggleOutline(bool state)
    {
        if (_outline == null) return;
        _outline.enabled = state;
    }

    public void Enter()
    {
        ToggleOutline(true);
        
        Debug.Log(10);
    }

    public void Exit()
    {
        ToggleOutline(false);
    }
}
