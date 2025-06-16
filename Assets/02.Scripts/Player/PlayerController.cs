using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region InputSystem
    public PlayerInputs playerInput { get; private set; }
    public PlayerInputs.PlayerActions playerActions { get; private set; }
    
    #endregion
    
    #region State Machine & State Flags
    public PlayerStateMachine stateMachine { get; private set; }
    
    
    [Header("State Flags")]
    public bool isWalking = false;
    public bool isWalkingHold => playerActions.Walk.IsPressed();
    public bool isMoving = false;
    public bool isCrouching = false;
    public bool isSprinting = false;
    public bool isSprintHold => playerActions.Sprint.IsPressed();
    public bool isReloading = false;
    public bool isReloadPressed => playerActions.Reloading.WasPressedThisFrame();
    public bool isAiming = false;
    public bool isAimHold => playerActions.Aiming.IsPressed();
    public bool isAttacking = false;
    public bool isJumping = false;
    public bool isJumpPressed => playerActions.Jump.WasPressedThisFrame();

    #endregion

    #region Camera Look
    [SerializeField, Range(-90f, 0f)] private float minXLook = -60f;
    [SerializeField, Range(0f, 90f)] private float maxXLook = 30f;
    [SerializeField, Range(50f, 300f)] private float lookSensitivity = 100f;
    public Transform playerTrans;
    private Transform camTrans;
    private float xRotation = 0f;
    #endregion

    #region Camera Move
    private Vector2 curMovementInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] public FPSVirtualCamera fpsVirtualCamera;
    #endregion

    #region Components
    public PlayerCondition Condition { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    #endregion
    
    private void Awake()
    {
        playerInput = new PlayerInputs();
        playerActions = playerInput.Player;

        Condition = GetComponent<PlayerCondition>();
        stateMachine = new PlayerStateMachine(this);
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        
        stateMachine.MainCamTransform = Camera.main?.transform;
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();
    
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
        // Player 시선에 따른 카메라 이동
        Look();
        
        // Crouch 판정 및 처리
        if (playerActions.Sit.WasPressedThisFrame())
        {
            isCrouching = !isCrouching; // 토글
            Crouch(isCrouching);
        }

        // Camera 특수 효과
        CameraMove();
        
        stateMachine.CurrentState.Update();
    }
    
    // Animation Clip 길이 구하기
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

    private void CameraMove()
    {
        curMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveSpeed = stateMachine.curMoveSpeed;
        
        if (Input.GetMouseButton(0))
        {
            fpsVirtualCamera.PlayRecoilToFire(Vector3.one);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            fpsVirtualCamera.ZoomIn(-20f, 0.5f);
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            fpsVirtualCamera.ZoomOut(0.5f);
        }

        if (curMovementInput != Vector2.zero)
        {
            fpsVirtualCamera.UpdateHeadBob(moveSpeed);
        }
        
        if (isSprintHold 
            && stateMachine.CurrentState is PlayerSprintState)
        {
            Debug.Log("Sprint Zoom In");
            fpsVirtualCamera.ZoomIn(20f, 0.5f);
        }
    }

    void Crouch(bool isCrouch)
    {
        if (isCrouch)
        {
            Debug.Log("Crouch");
            // 추후 isCrouching 값에 따라 조절하도록 구조 조정할 것
            Animator.SetBool("isCrouching", true);
            Controller.height = 1.1f;
            Controller.center = new Vector3(0, 0.6f, 0f);
        }
        else
        {
            Debug.Log("UnCrouch");
            Animator.SetBool("isCrouching", false);
            Controller.height = 1.9f;
            Controller.center = new Vector3(0, 1f, 0f);
        }
        
        // Crouch일 때의 카메라 이동 추가
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
