using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected PlayerStateMachine _stateMachine;
    protected PlayerController _controller;
    
    public State(PlayerStateMachine stateMachine, PlayerController controller)
    {
        this._stateMachine = stateMachine;
        this._controller = controller;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
