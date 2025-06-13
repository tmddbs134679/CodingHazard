using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    public PlayerAimingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _controller.Animator.SetBool("isAiming", true);
        _controller.Animator.SetBool("isReloading", false);
    }

    public override void Update()
    {
        base.Update();
        if (_controller.playerActions.Aiming.ReadValue<float>() == 0f)
        {
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
        }
    }

    public override void Exit()
    {
        base.Exit();
        _controller.Animator.SetBool("isAiming", false);
    }
}
