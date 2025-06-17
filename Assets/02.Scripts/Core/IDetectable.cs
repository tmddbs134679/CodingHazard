using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetectable
{
    public bool IsLockDetect { get; set; }

    public void Enter();
    public void Exit();
}
