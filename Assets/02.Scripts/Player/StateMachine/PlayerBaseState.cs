using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState : State
{
    protected PlayerBaseState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public virtual void OnAttackInput() { }

    protected Vector2 GetMovementInput()
    {
        return _controller.playerActions.Movement.ReadValue<Vector2>();
    }
    
    protected Vector3 GetMoveDirection(Vector2 input)
    {
        Vector3 forward = _stateMachine.FPScamTransform.forward;
        Vector3 right = _stateMachine.FPScamTransform.right;
        forward.y = 0f;
        right.y = 0f;

        Vector3 moveDir = forward.normalized * input.y + right.normalized * input.x;

        return moveDir;
    }
    
    protected void Move(Vector3 direction, float speed)
    {
        _controller.stateMachine.curMoveSpeed = speed;
        _controller.Controller.Move(((direction * speed) + _controller.ForceReceiver.Movement) * Time.deltaTime);
    }
    
    protected bool IsAttackTriggered()
    {
        return _controller.playerActions.Attack.triggered;
    }
}
