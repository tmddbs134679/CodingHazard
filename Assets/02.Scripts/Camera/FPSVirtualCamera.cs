using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FPSVirtualCamera : MonoBehaviour
{
    public Vector2 MouseDelta { get; set; }


    [SerializeField] private LayerMask dofLayerMask;

    
    [SerializeField] private float headbobFrequency = 10f;
    [SerializeField] private float headbobAmplitude = 0.05f;
    
    [Space(10f)]
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    
    [Space(10f)]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraRoot;
    
    [Space(10f)]
    [SerializeField] private CinemachineImpulseSource hitImpulseSource;
    [SerializeField] private UniversalAdditionalCameraData urpCameraData;
    

    private CinemachineVirtualCamera _virtualCamera;
    private GameSettingManager _gameSettingManager;
    private DepthOfField _dof;
    
    private Vector3 _defaultLocalPos;
    private Vector3 _curRecoil;
    private Vector3 _crouchAdjustmentPos;
    private Vector3 _headBobAdjustmentPos;


    private float _defaultFOV;
    private float _camCurXRot;
    private float _lookSensitivity;


    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();

        _defaultFOV = _virtualCamera.m_Lens.FieldOfView;
    }
    

    private void Start()
    {
        _gameSettingManager = GameManager.Instance.GraphicsSettingManager;

        if (_gameSettingManager.TryGetVolumeComponent(out DepthOfField dof))
        {
            _dof = dof;
        }

        _gameSettingManager.OnChangedFOV += ChangeFOV;
        
        _gameSettingManager.OnChangedAntialiasingMode += ChangeAntiMode;

        _gameSettingManager.OnChangedMouseSensitivity += ChangeMouseSensitivity;
        
        PlayerEvent.OnTakeDamaged += PlayHitFeedback;

        
        ChangeFOV(_gameSettingManager.FOV);
        ChangeAntiMode(_gameSettingManager.AntialiasingMode);
        ChangeMouseSensitivity(_gameSettingManager.MouseSensitivity);
        
        
        _defaultLocalPos = cameraRoot.localPosition;

    }

    private void OnDestroy()
    {
        _gameSettingManager.OnChangedFOV -= ChangeFOV;
        
        _gameSettingManager.OnChangedAntialiasingMode -= ChangeAntiMode;

        _gameSettingManager.OnChangedMouseSensitivity -= ChangeMouseSensitivity;
        
        PlayerEvent.OnTakeDamaged -= PlayHitFeedback;
    }


    private void Update()
    {
        //MouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); 

        if (_dof != null)
        {
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1000f, dofLayerMask);

            if (hit.collider != null)
            {
                _dof.focusDistance.value = hit.distance;
            }
            else
            {
                _dof.focusDistance.value = 100;
            }
        }
        
    }
    
    
    private void LateUpdate()
    {
        LateUpdateCamera();

        _curRecoil = Vector3.zero;
    }

    
    public void ZoomIn(float zoomValue, float duration)
    {
        float targetFov = _defaultFOV + zoomValue;
        
        DOTween.To(
            () => _virtualCamera.m_Lens.FieldOfView,
            x => _virtualCamera.m_Lens.FieldOfView = x,
            targetFov,
            duration);
    }
    
    public void ZoomOut(float duration)
    {
        DOTween.To(
            () => _virtualCamera.m_Lens.FieldOfView,
            x => _virtualCamera.m_Lens.FieldOfView = x,
            _defaultFOV,
            duration);
    }
    
  
    public void UpdateHeadBob(float ratio)
    {
        float frequency = headbobFrequency * Mathf.Lerp(0.5f, 1.5f, ratio);

        _headBobAdjustmentPos = new Vector3(
            Mathf.Cos(Time.time * frequency * 0.5f) * headbobAmplitude * 0.5f,
            Mathf.Sin(Time.time * frequency) * headbobAmplitude,
            0
        );
    }
    
    public void PlayHitFeedback() => hitImpulseSource.GenerateImpulse();
    
    public void PlayRecoilToFire(Vector3 recoil) => _curRecoil = new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), -recoil.z);

    public void SetCrouchAdjustment(Vector3 targetAdjustmentPos)
    {
        _crouchAdjustmentPos = targetAdjustmentPos;
    }

    private void ChangeAntiMode(AntialiasingMode mode) => urpCameraData.antialiasing = mode;
    
    private void ChangeMouseSensitivity(float mouseSensitivity) => _lookSensitivity = mouseSensitivity;
    
    private void ChangeFOV(float fov)
    {
        _virtualCamera.m_Lens.FieldOfView = fov;

        _defaultFOV = fov;
    }

    
    private void LateUpdateCamera()
    {
        MouseDelta *= _lookSensitivity;
        
        var camDelta = new Vector3(MouseDelta.y, MouseDelta.x, 0);
        
        Vector3 totalMouseDelta = camDelta + _curRecoil; 
        
        _camCurXRot -= totalMouseDelta.x;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook); 
        
        cameraRoot.localRotation = Quaternion.Euler(_camCurXRot, 0f, 0f);
        cameraRoot.localPosition = _defaultLocalPos + _crouchAdjustmentPos + _headBobAdjustmentPos;


        playerTransform.Rotate(Vector3.up * totalMouseDelta.y);
    }
    
}
