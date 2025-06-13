using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReloadingState : State
{
    private float timer; 
    
    public PlayerReloadingState(PlayerStateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _controller.Animator.SetBool("isReloading", true);
        _controller.Animator.SetTrigger("Reload");

        // 애니메이션 길이 구하기 현재 오류 있어서 제대로 작동 안 함
        float reloadDuration = _controller.GetAnimationClipLength("Reloading");
        timer = reloadDuration;
        Debug.Log(timer);
    }

    public override void Update()
    {
        base.Update();
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine, _controller));
        }
    }
    
    public override void Exit()
    {
        _controller.Animator.SetBool("isReloading", false);
    }
}
