using UnityEngine;

public class PlayerStateMachine
{
    public PlayerController Controller { get; private set; }
    public PlayerBaseState CurrentState { get; private set; }
    
    public Transform MainCamTransform { get; set; }
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; set; } = 5f;
    public float WalkSpeed { get; set; } = 2.5f;
    public float SprintSpeed { get; set; } = 10f;
    public float MovementSpeedModifier { get; set; } = 1f;

    public PlayerStateMachine(PlayerController controller)
    {
        Controller = controller;
    }

    public void Initialize(PlayerBaseState state)
    {
        CurrentState = state;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerBaseState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    private void Update()
    {
        CurrentState?.Update();
    }

    private void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}
