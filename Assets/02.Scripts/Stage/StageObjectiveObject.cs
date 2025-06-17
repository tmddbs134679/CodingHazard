using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageObjectiveObject : MonoBehaviour
{
    public abstract StageObjectiveType ObjectiveType { get; }
    public bool IsLockInteract { get; private set; }
    
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }
    
    public void ToggleLockInteract(bool isLock)
    {
        IsLockInteract = isLock;
    }
    
    public void ToggleOutline(bool state)
    {
        if (_outline == null) return;
        _outline.enabled = state;
    }
}
