using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State
{
    public PlayerIdleState(PlayerStateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void Enter()
    {
        _controller.Animator.SetBool("isAiming", false);
        _controller.Animator.SetBool("isReloading", false);
    }

    public override void Update()
    {
        if (_controller.playerActions.Reloading.WasPressedThisFrame())
        {
            Debug.Log("Change State to Reloading");
            _stateMachine.ChangeState(new PlayerReloadingState(_stateMachine, _controller));
            return;
        }
        
        if (_controller.playerActions.Aiming.ReadValue<float>() > 0)
        {
            Debug.Log("Change State to Aiming");
            _stateMachine.ChangeState(new PlayerAimingState(_stateMachine, _controller));
        }
    }
    
    
}
