using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 이동
public class PlayerMoveState : PlayerBaseState
{
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
        
        // Jump 가능
        if (_stateMachine.Controller.isJumpPressed)
        {
            _stateMachine.ChangeState(new PlayerJumpState(_stateMachine));
            return;
        }
        
        // sprint 가능
        if (_stateMachine.Controller.isSprintHold
            && _stateMachine.Controller.Condition.UseStamina(_stateMachine.SprintStamina))
        {
            PlayerEvent.OnStaminaChanged?.Invoke(_stateMachine.SprintStamina);
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
