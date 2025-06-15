using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter Fall State");
    }

    public override void Update()
    {
        Vector2 input = GetMovementInput();
        Vector3 dir = GetMoveDirection(input);
        // 점프 땐 좀 느리게 이동 가능
        Move(dir, _stateMachine.WalkSpeed);
        
        if (_stateMachine.Controller.Controller.isGrounded)
        {
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
        }
    }
}
