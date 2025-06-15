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
        _stateMachine.Controller.isSprinting = true;
        Debug.Log("Enter SprintState");
        time = 0f;

        if (_controller.isCrouching)
        {
            _controller.isCrouching = false;
        }
    }

    public override void Update()
    {
        time += Time.deltaTime;
        sprintDirection = _stateMachine.Controller.playerTrans.forward;
        if (time >= sprintDuration)
        {
            _stateMachine.Controller.isSprinting = false;
            // Sprint 끝나면 Move 상태로 전환
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
            return;
        }
        
        Move(sprintDirection, _stateMachine.SprintSpeed);
        
        if (_stateMachine.Controller.isJumpPressed)
        {
            _stateMachine.ChangeState(new PlayerJumpState(_stateMachine));
            return;
        }

        if (!_controller.isSprintHold)
        {
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
        }
    }
}
