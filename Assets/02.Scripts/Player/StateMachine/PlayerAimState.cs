using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimState : PlayerBaseState
{
    public PlayerAimState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter Aim State"); 
        _stateMachine.Controller.fpsVirtualCamera.ZoomIn(-20f, 0.5f);
    }

    public override void Update()
    {
        base.Update();
        
        // aim 상태로 느린 걸음 가능
        Vector2 input = GetMovementInput();
        Vector3 dir = GetMoveDirection(input);
        Move(dir, _stateMachine.WalkSpeed);
        
        // 안 누르고 있으면 idle로
        if (!_controller.isAimHold)
        {
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
        }
        
        // 사격 가능
        if (IsAttackTriggered())
        {
            OnAttackInput();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.Controller.fpsVirtualCamera.ZoomOut(0.5f);
    }

    public override void OnAttackInput()
    {
        _controller.Attack();
    }
}
