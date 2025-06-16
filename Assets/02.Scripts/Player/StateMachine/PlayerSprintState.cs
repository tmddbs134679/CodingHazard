using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    private float stamina;
    public float sprintDuration;
    private float time = 0f;
    private Vector3 sprintDirection;
    
    public PlayerSprintState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter SprintState");
        
        _stateMachine.Controller.isSprinting = true;
        stamina = _stateMachine.Controller.Condition.stamina.curValue;
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
        if (stamina <= 0)
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
