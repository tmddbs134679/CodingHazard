using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerInput { get; private set; }
    public PlayerInputs.PlayerActions playerActions { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }
    
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    
    private void Awake()
    {
        playerInput = new PlayerInputs();
        playerActions = playerInput.Player;
        stateMachine = new PlayerStateMachine(this);
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        stateMachine.MainCamTransform = Camera.main?.transform;
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
        
        stateMachine.Initialize(new PlayerIdleState(stateMachine));
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

    public void Attack()
    {
        Debug.Log("PlayerController Attack Method");
        /*  // 현재 무기가 근접 무기일 경우 MeleeAttackState
         * if (현재 무기 == 근접 무기)
         *      stateMachine.ChangeState(new PlayerMeleeAttackState(stateMachine));
        
            // 주/보조 무기(총)일 경우 총을 쏘는 Method 실행
        */
    }
}
