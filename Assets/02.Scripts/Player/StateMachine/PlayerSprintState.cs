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
        
        _stateMachine.Controller.fpsVirtualCamera.ZoomIn(20f, 0.5f);

        PlayerEvent.OnSprint?.Invoke(true);
    }

    public override void Update()
    {
        _stateMachine.Controller.Condition.UseStamina(_stateMachine.SprintStamina);
        
        sprintDirection = _stateMachine.Controller.playerTrans.forward;
        Move(sprintDirection, _stateMachine.SprintSpeed);
        
        _stateMachine.Controller.fpsVirtualCamera.UpdateHeadBob(_stateMachine.SprintSpeed);
        
        if (_stateMachine.Controller.Condition.stamina.curValue <= 0)
        {
            // Sprint 끝나면 Idle 상태로 전환
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
            return;
        }
        
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

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Sprint Zoom Out");
        _stateMachine.Controller.isSprinting = false;
        _stateMachine.Controller.fpsVirtualCamera.ZoomOut(0.5f);
        PlayerEvent.OnSprint?.Invoke(false);
    }
}
