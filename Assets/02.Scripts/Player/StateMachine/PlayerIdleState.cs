using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter IdleState");
    }

    public override void Update()
    {
        Vector2 input = GetMovementInput();

        // wasd input 들어온 건지 확인하고 change State
        if (input.magnitude > 0.1f)
        {
            // move가 기본, walk가 슬로우, run은 뛰기, dash는 질주
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
            return;
        }

        if (_stateMachine.Controller.isJumpPressed)
        {
            _stateMachine.ChangeState(new PlayerJumpState(_stateMachine));
            return;
        }
        
        // 장전 가능
        if (_stateMachine.Controller.isReloadPressed)
        {
            _stateMachine.Controller.isMoving = false;
            _stateMachine.ChangeState(new PlayerReloadState(_stateMachine));    
        }
        
        // Aim 가능
        if (_stateMachine.Controller.isAimHold)
        {
            _stateMachine.Controller.isMoving = false;
            _stateMachine.ChangeState(new PlayerAimState(_stateMachine));
        }

        if (IsAttackTriggered())
        {
            OnAttackInput();
        }
    }

    public override void OnAttackInput()
    {
        _controller.Attack();
    }
}
