using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerInput { get; private set; }
    public PlayerInputs.PlayerActions playerActions { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }
    
    public Animator Animator { get; private set; }
    
    private void Awake()
    {
        playerInput = new PlayerInputs();
        playerActions = playerInput.Player;
        stateMachine = new PlayerStateMachine();
        Animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    private void Start()
    {
        stateMachine.Initialize(new PlayerIdleState(stateMachine, this));
    }

    private void Update()
    {
        stateMachine.CurrentState.Update();
    }
    
    public float GetAnimationClipLength(string clipName)
    {
        var clips = Animator.runtimeAnimatorController.animationClips;

        foreach (var clip in clips)
        {
            if (clip.name == clipName)
                return clip.length;
        }

        Debug.LogWarning($"Clip '{clipName}' not found!");
        return 2f; // fallback
    }
}
