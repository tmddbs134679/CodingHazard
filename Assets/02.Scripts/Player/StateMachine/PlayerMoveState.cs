using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 이동
public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateMachine.Controller.isMoving = true;
        Debug.Log("Enter MoveState");
    }

    public override void Update()
    {
        Vector2 input = GetMovementInput();
        Vector3 dir = GetMoveDirection(input);
        Move(dir, _stateMachine.MovementSpeed);

        // wasd input 들어온 건지 확인
        if (input.magnitude <= 0.1f)
        {
            // 입력 안 들어온 거면 Idle
            _stateMachine.Controller.isMoving = false;
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
            return;
        }
        
        // Jump 가능
        if (_stateMachine.Controller.isJumpPressed)
        {
            _stateMachine.ChangeState(new PlayerJumpState(_stateMachine));
            return;
        }
        
        // sprint 가능
        if (_stateMachine.Controller.isSprintHold)
        {
            _stateMachine.Controller.isMoving = false;
            _stateMachine.ChangeState(new PlayerSprintState(_stateMachine));
        }

        // 걷기 가능
        if (_stateMachine.Controller.isWalkingHold)
        {
            _stateMachine.Controller.isMoving = false;
            _stateMachine.ChangeState(new PlayerWalkState(_stateMachine));    
        }
        
        // 앉기 가능
        if (_controller.playerActions.Sit.IsPressed())
        {
            _stateMachine.Controller.isMoving = false;
            _stateMachine.ChangeState(new PlayerSitState(_stateMachine));
        }

        // 사격 가능
        if (IsAttackTriggered())
        {
            OnAttackInput();
        }
    }
}
