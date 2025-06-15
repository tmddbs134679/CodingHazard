using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 느린 걸음
public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _controller.isWalking = true;
        Debug.Log("Enter Walk State");
    }

    public override void Update()
    {
        Vector2 input = GetMovementInput();
        Vector3 dir = GetMoveDirection(input);
        
        if (_stateMachine.Controller.isCrouching)
        {
            Move(dir, _stateMachine.WalkSpeed / 2);
        }
        else
        {
            Move(dir, _stateMachine.WalkSpeed);
        }
        
        // wasd input 들어온 건지 확인하고 change State
        if (input.magnitude <= 0.1f)
        {
            // 입력 안 들어온 거면 Idle
            _stateMachine.Controller.isWalking = false;
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
            return;
        }
        
        // ctrl 안 누르고 있으면 move로 돌아감
        if (!_stateMachine.Controller.isWalkingHold)
        {
            _stateMachine.Controller.isWalking = false;
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
        }
        
        if (_stateMachine.Controller.isJumpPressed)
        {
            _stateMachine.Controller.isWalking = false;
            _stateMachine.ChangeState(new PlayerJumpState(_stateMachine));
        }
        
        // shift 
        if (_stateMachine.Controller.isSprintHold) 
        {
            _stateMachine.Controller.isWalking = false;
            _stateMachine.ChangeState(new PlayerSprintState(_stateMachine));
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
