using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float jumpForce = 5f;
    private float jumpStartTime;
    
    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateMachine.Controller.ForceReceiver.Jump(jumpForce);
        _stateMachine.Controller.isJumping = true;
        jumpStartTime = Time.time;
        
        Debug.Log("Enter Jump State");
        
        if (_controller.isCrouching)
        {
            _controller.isCrouching = false;
        }
    }

    public override void Update()
    {
        base.Update();
        
        Vector2 input = GetMovementInput();
        Vector3 dir = GetMoveDirection(input);
        // 점프 땐 좀 느리게 이동 가능
        Move(dir, _stateMachine.WalkSpeed);
        
        // 바닥 인식에 delay를 줘서 점프가 안 되는 현상 방지
        bool hasBeenAirborneLongEnough = Time.time > jumpStartTime + 0.15f;

        // 땅에 닿아있고 jump가 true면 (점프해서 어딘가에 올라간 경우) Idle로
        if (hasBeenAirborneLongEnough 
            && _stateMachine.Controller.Controller.isGrounded 
            && _stateMachine.Controller.isJumping)
        {
            _stateMachine.Controller.isJumping = false;
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
        }
        
        // 떨어질 때
        if (_stateMachine.Controller.ForceReceiver.Movement.y <= 0f)
        {
            _stateMachine.Controller.isJumping = false;
            _stateMachine.ChangeState(new PlayerFallState(_stateMachine));
        }


    }
}
