using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 기본 이동
public class PlayerMoveState : PlayerBaseState
{
    private float moveTimer;
    private float moveInterval = 0.35f;
    private bool useMoveSound = true;

    
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateMachine.Controller.isMoving = true;
        Debug.Log("Enter MoveState");
    }

    public override void Update()
    {
        Vector2 input = GetMovementInput();
        Vector3 dir = GetMoveDirection(input);

        if (input.magnitude > 0.1f)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0f)
            {
                if (useMoveSound)
                {
                    AudioManager.Instance.PlayAudio(AudioID.PlayerWalk1);
                }
                else
                {
                    AudioManager.Instance.PlayAudio(AudioID.PlayerWalk2);
                }
                useMoveSound = !useMoveSound;
                moveTimer = moveInterval;
            }
        }
        
        // 앉았는지 아닌지
        if (_stateMachine.Controller.isCrouching)
        {
            Move(dir, _stateMachine.MovementSpeed / 2);
            _stateMachine.Controller.fpsVirtualCamera.UpdateHeadBob(_stateMachine.MovementSpeed / 2);
        }
        else
        {
            Move(dir, _stateMachine.MovementSpeed);
            _stateMachine.Controller.fpsVirtualCamera.UpdateHeadBob(_stateMachine.MovementSpeed);
        }
        
        // wasd input 들어온 건지 확인
        if (input.magnitude <= 0.1f)
        {
            // 입력 안 들어온 거면 Idle
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
            return;
        }
        
        // if (!_stateMachine.Controller.canSprint 
        //     && _stateMachine.Controller.Condition.stamina.curValue 
        //         >= _stateMachine.SprintStamina)
        // {
        //     _stateMachine.Controller.canSprint = true;
        // }
        
        // Jump 가능
        if (_stateMachine.Controller.isJumpPressed)
        {
            _stateMachine.ChangeState(new PlayerJumpState(_stateMachine));
            return;
        }
        
        // sprint 가능
        if (_stateMachine.Controller.isSprintHold && _stateMachine.Controller.canSprint)
        {
            _stateMachine.ChangeState(new PlayerSprintState(_stateMachine));
        }
        
        // Aim 가능
        if (_stateMachine.Controller.isAimHold)
        {
            _stateMachine.ChangeState(new PlayerAimState(_stateMachine));
        }

        // 걷기 가능
        if (_stateMachine.Controller.isWalkingHold)
        {
            _stateMachine.ChangeState(new PlayerWalkState(_stateMachine));    
        }
        
        // 장전 가능
        if (_stateMachine.Controller.isReloadPressed)
        {
            _stateMachine.ChangeState(new PlayerReloadState(_stateMachine));    
        }

        // 사격 가능
        if (IsAttackTriggered())
        {
            OnAttackInput();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.Controller.isMoving = false;
    }

    public override void OnAttackInput()
    {
        //이 상태일때 다르게 동작할때 무언가?
        _controller.Attack();
    }
}
