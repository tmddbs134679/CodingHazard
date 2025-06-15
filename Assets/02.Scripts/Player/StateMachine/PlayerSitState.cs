using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSitState : PlayerBaseState
{
    public PlayerSitState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter SitState");
    }

    public override void Update()
    {
        // 앉아서 기본 이동 sitMove
        
        // 앉아서 걷기 sitWalk
        
        // 앉아서 사격
        if (IsAttackTriggered())
        {
            OnAttackInput();
        }
        
        // 점프 가능
        
        // 질주 가능
        if (_controller.playerActions.Sprint.IsPressed())
        {
            _stateMachine.ChangeState(new PlayerSprintState(_stateMachine));
        }
    }
}
