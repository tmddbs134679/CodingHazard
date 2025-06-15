using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    private float sprintDuration = 1f;
    private float time = 0f;
    private Vector3 sprintDirection;
    
    public PlayerSprintState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _controller.isSprinting = true;
        Debug.Log("Enter SprintState");
        time = 0f;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        sprintDirection = _controller.playerTrans.forward;
        if (time >= sprintDuration)
        {
            _controller.isSprinting = false;
            // Sprint 끝나면 Move 상태로 전환
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
            return;
        }
        
        Move(sprintDirection, _stateMachine.SprintSpeed);
        
        if (_controller.playerActions.Sit.IsPressed())
        {
            _controller.isSprinting = false;
            _stateMachine.ChangeState(new PlayerSitState(_stateMachine));
        }

        if (!_controller.isSprintHold)
        {
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
        }
    }
}
