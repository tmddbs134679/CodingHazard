using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 느린 걸음
public class PlayerWalkState : PlayerBaseState
{
    private float walkTimer;
    private float walkInterval = 0.5f;
    private bool useWalkSound = true;
    
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _controller.isWalking = true;
        Debug.Log("Enter Walk State");
    }

    public override void Update()
    {
        Vector2 input = GetMovementInput();
        Vector3 dir = GetMoveDirection(input);
        
        if (input.magnitude > 0.1f)
        {
            walkTimer -= Time.deltaTime;
            if (walkTimer <= 0f)
            {
                if (useWalkSound)
                {
                    AudioManager.Instance.PlayAudio(AudioID.PlayerWalk1, 0.025f);
                }
                else
                {
                    AudioManager.Instance.PlayAudio(AudioID.PlayerWalk2, 0.025f);
                }
                useWalkSound = !useWalkSound;
                walkTimer = walkInterval;
            }
        }
        
        if (_stateMachine.Controller.isCrouching)
        {
            Move(dir, _stateMachine.WalkSpeed / 2);
            _stateMachine.Controller.fpsVirtualCamera.UpdateHeadBob(_stateMachine.WalkSpeed / 2);
        }
        else
        {
            Move(dir, _stateMachine.WalkSpeed);
            _stateMachine.Controller.fpsVirtualCamera.UpdateHeadBob(_stateMachine.WalkSpeed);
        }
        
        // wasd input 들어온 건지 확인하고 change State
        if (input.magnitude <= 0.1f)
        {
            // 입력 안 들어온 거면 Idle
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
            return;
        }
        
        // ctrl 안 누르고 있으면 move로 돌아감
        if (!_stateMachine.Controller.isWalkingHold)
        {
            _stateMachine.ChangeState(new PlayerMoveState(_stateMachine));
        }
        
        if (_stateMachine.Controller.isJumpPressed)
        {
            _stateMachine.ChangeState(new PlayerJumpState(_stateMachine));
        }
        
        // Aim 가능
        if (_stateMachine.Controller.isAimHold)
        {
            if (_controller.WeaponManager.CurrentWpeaWeapon is Gun)
            {
                _stateMachine.Controller.isMoving = false;
                _stateMachine.ChangeState(new PlayerAimState(_stateMachine));
            }
        }
        
        // shift 
        if (_stateMachine.Controller.isSprintHold && _stateMachine.Controller.canSprint)
        {
            _stateMachine.ChangeState(new PlayerSprintState(_stateMachine));
        }
        
        // 장전 가능
        if (_stateMachine.Controller.isReloadPressed)
        {
            _stateMachine.ChangeState(new PlayerReloadState(_stateMachine));    
        }

        if (IsAttackTriggered())
        {
            OnAttackInput();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.Controller.isWalking = false;
    }

    public override void OnAttackInput()
    {
        _controller.Attack();
    }
}
