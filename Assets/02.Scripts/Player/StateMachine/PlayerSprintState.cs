using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    private Vector3 sprintDirection;
    public PlayerSprintState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter SprintState");
        
        _stateMachine.Controller.isSprinting = true;

        if (_controller.isCrouching)
        {
            _controller.isCrouching = false;
        }
    }

    public override void Update()
    {
        _stateMachine.Controller.Condition.stamina.curValue -= _stateMachine.SprintStamina * Time.deltaTime;
        sprintDirection = _stateMachine.Controller.playerTrans.forward;
        
        Move(sprintDirection, _stateMachine.SprintSpeed);
        
        if (_stateMachine.Controller.Condition.stamina.curValue <= 0)
        {
            _stateMachine.Controller.isSprinting = false;
            // Sprint 끝나면 Idle 상태로 전환
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
            return;
        }
        
        if (_stateMachine.Controller.isJumpPressed)
        {
            _stateMachine.Controller.isSprinting = false;
            _stateMachine.ChangeState(new PlayerJumpState(_stateMachine));
            return;
        }

        if (!_controller.isSprintHold)
        {
            _stateMachine.Controller.isSprinting = false;
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Sprint Zoom Out");
        _stateMachine.Controller.fpsVirtualCamera.ZoomOut(0.5f);
    }
}
