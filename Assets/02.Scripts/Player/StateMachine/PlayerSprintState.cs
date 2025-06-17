using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    private Vector3 sprintDirection;
    
    private float sprintTimer;
    private float sprintInterval = 0.25f;
    private bool useSprintSound = true;
    
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
        
        if (_stateMachine.Controller.WeaponManager.CurrentWpeaWeapon is Gun)
        {
            _stateMachine.Controller.fpsVirtualCamera.ZoomIn(-20f, 0.5f);
        }

        PlayerEvent.OnSprint?.Invoke(true);
    }

    public override void Update()
    {
        sprintTimer -= Time.deltaTime;
        if (sprintTimer <= 0f)
        {
            if (useSprintSound)
            {
                AudioManager.Instance.PlayAudio(AudioID.PlayerWalk1);
            }
            else
            {
                AudioManager.Instance.PlayAudio(AudioID.PlayerWalk2);
            }
            useSprintSound = !useSprintSound;
            sprintTimer = sprintInterval;
        }
        
        
        if (_stateMachine.Controller.Condition.stamina.curValue <= 0f)
        {
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
            return;
        }
        
        _stateMachine.Controller.Condition.UseStamina(_stateMachine.SprintStamina);
        
        
        sprintDirection = _stateMachine.Controller.playerTrans.forward;
        Move(sprintDirection, _stateMachine.SprintSpeed);
        
        _stateMachine.Controller.fpsVirtualCamera.UpdateHeadBob(_stateMachine.SprintSpeed);
        
        
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
        
        if (_stateMachine.Controller.WeaponManager.CurrentWpeaWeapon is Gun)
        {
            _stateMachine.Controller.fpsVirtualCamera.ZoomOut(0.5f);
        }
     
        PlayerEvent.OnSprint?.Invoke(false);
    }
}
