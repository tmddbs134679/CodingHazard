using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerBaseState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public virtual void OnAttackInput() { }
    public virtual void OnJumpInput() { }
    public virtual void OnDashInput() { }
    public virtual void OnSitInput() { }
    public virtual  void OnMoveInput(Vector2 input) { }

    protected Vector3 GetMoveDirection(Vector2 input)
    {
        Vector3 forward = _stateMachine.MainCamTransform.forward;
        Vector3 right = _stateMachine.MainCamTransform.right;
        forward.y = 0f;
        right.y = 0f;

        Vector3 moveDir = forward.normalized * input.y + right.normalized * input.x;

        return moveDir;
    }
    
    protected void Move(Vector3 direction, float speed)
    {
        _controller.Controller.Move(direction * speed * Time.deltaTime);
    }

    protected Vector2 GetMovementInput()
    {
        return _controller.playerActions.Movement.ReadValue<Vector2>();
    }

    protected bool IsAttackTriggered()
    {
        return _controller.playerActions.Attack.triggered;
    }
}
