using UnityEngine;

public class PlayerReloadState : PlayerBaseState
{
    private float timer; 
    
    public PlayerReloadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter Reload State");
        _stateMachine.Controller.isReloading = true;
        timer = 2f; // 장전 시간 설정 / 후에 아마 애니메이션 길이로 설정
        PlayerEvent.OnReLoad?.Invoke();
    }

    public override void Update()
    {
        base.Update();
        
        // 느린 속도로 이동 가능
        Vector2 input = GetMovementInput();
        Vector3 dir = GetMoveDirection(input);
        Move(dir, _stateMachine.WalkSpeed);
        
        // 장전 다 하면 idle
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine));
        }
        
        // 장전 중에 가능한 state 추후 추가
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.Controller.isReloading = false;
    }
}
