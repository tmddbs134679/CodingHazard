using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class FPSVirtualCamera : MonoBehaviour
{
    public CinemachineImpulseSource HitImpulseSource => hitImpulseSource;

    [SerializeField] private float headbobFrequency = 10f;
    [SerializeField] private float headbobAmplitude = 0.05f;
    
    [Space(10f)]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraRoot;
    
    [Space(10f)]
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    
    [Space(10f)]
    [SerializeField] private CinemachineImpulseSource hitImpulseSource;

    
    private CinemachineVirtualCamera _virtualCamera;
    
    private Vector3 _defaultLocalPos;
    private Vector3 _curRecoil;
    private Vector2 _mouseDelta;

    private float _defaultFOV;
    private float _camCurXRot;
    private float _mouseSensitivity;


    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        _defaultFOV = _virtualCamera.m_Lens.FieldOfView;
    }

    private void Start()
    {
        _defaultLocalPos = cameraRoot.localPosition;
    }


    private void LateUpdate()
    {
        CameraLook();

        _curRecoil = Vector3.zero;
        
        cameraRoot.localPosition = Vector3.Lerp(cameraRoot.localPosition, _defaultLocalPos, Time.deltaTime * 5f);
    }


    public void PlayRecoilToFire(Vector3 recoil)
    {
        _curRecoil = new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
    }

    public void SetFOV(float value)
    {
        value = Mathf.Clamp(value, Constants.MinFOV, Constants.MaxFOV);

        _virtualCamera.m_Lens.FieldOfView = value;

        _defaultFOV = value;
    }

    public void SetMouseSensitivity(float value)
    {
        value = Mathf.Clamp(value, Constants.MinFOV, Constants.MaxFOV);
        
        _mouseSensitivity = value;
    }


    public void SetLookMouseDelta(Vector2 delta)
        => _mouseDelta = delta;
    

    public void ZoomIn(float zoomValue, float duration = 0.5f)
    {
        float targetFov = _defaultFOV + zoomValue;
        
        DOTween.To(
            () => _virtualCamera.m_Lens.FieldOfView,
            x => _virtualCamera.m_Lens.FieldOfView = x,
            targetFov,
            duration);
    }
    
    
    public void ZoomOut(float duration = 0.5f)
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
        
        float headbobY = Mathf.Sin(Time.time * frequency) * headbobAmplitude;
        float headbobX = Mathf.Cos(Time.time * frequency * 0.5f) * headbobAmplitude * 0.5f;
            
        cameraRoot.localPosition = _defaultLocalPos + new Vector3(headbobX, headbobY, 0);
    }
    
    
    private void CameraLook()
    {
        var camDelta = new Vector3(_mouseDelta.y, _mouseDelta.x, 0);
        
        Vector3 totalMouseDelta = camDelta * _mouseSensitivity + _curRecoil; 
        
        _camCurXRot -= totalMouseDelta.x;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook); 
        
        cameraRoot.localRotation = Quaternion.Euler(_camCurXRot, 0f, 0f);

        playerTransform.Rotate(Vector3.up * totalMouseDelta.y);
    }
    
}
