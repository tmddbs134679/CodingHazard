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
        Debug.Log("Enter SprintState");
        time = 0f;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        if (time >= sprintDuration)
        {
            // Sprint 끝나면 Move 상태로 전환
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
            return;
        }
        
        Move(sprintDirection, _stateMachine.SprintSpeed);
        

        if (_controller.playerActions.Sit.IsPressed())
        {
            _stateMachine.ChangeState(new PlayerSitState(_stateMachine));
        }
    }
}
