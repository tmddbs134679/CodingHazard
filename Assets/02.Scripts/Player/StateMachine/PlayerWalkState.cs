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
        Debug.Log("Enter WalkState");
    }

    public override void Update()
    {
        Vector2 input = GetMovementInput();
        Vector3 dir = GetMoveDirection(input);
        Move(dir, _stateMachine.WalkSpeed);

        // wasd input 들어온 건지 확인하고 change State
        if (input.magnitude <= 0.1f)
        {
            // 입력 안 들어온 거면 Idle
            _controller.isWalking = false;
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
            return;
        }

        if (!_controller.isWalkingHold)
        {
            _controller.isWalking = false;
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
        }
        
        // shift 누르면 대시
        if (_controller.playerActions.Sprint.IsPressed()) 
        {
            _controller.isWalking = false;
            _stateMachine.ChangeState(new PlayerSprintState(_stateMachine));
        }

        // C 누르면 앉기
        if (_controller.playerActions.Sit.IsPressed())
        {
            _controller.isWalking = false;
            _stateMachine.ChangeState(new PlayerSitState(_stateMachine));
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
