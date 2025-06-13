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

    [SerializeField, Range(-90f, 0f)] private float minXLook = -60f;
    [SerializeField, Range(0f, 90f)] private float maxXLook = 30f;
    [SerializeField, Range(50f, 300f)] private float lookSensitivity = 100f;
    private Transform playerTrans;
    private Transform camTrans;
    private float xRotation = 0f;
    
    
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        camTrans = stateMachine.MainCamTransform;
        playerTrans = transform;
        
        stateMachine.Initialize(new PlayerIdleState(stateMachine));
    }

    private void Update()
    {
        Look();
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
    
    private void Look()
    {
        Vector2 mouseDelta = playerActions.Look.ReadValue<Vector2>();
    
        float mouseX = mouseDelta.x * lookSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * lookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minXLook, maxXLook); // 위 아래 제한

        camTrans.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // 상하
        playerTrans.Rotate(Vector3.up * mouseX); // 좌우
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
