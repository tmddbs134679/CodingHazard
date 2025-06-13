using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
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
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
            return;
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
