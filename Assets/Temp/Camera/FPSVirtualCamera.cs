using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class FPSVirtualCamera : MonoBehaviour
{
    [SerializeField] private float testRatio;
    
    [Space(10f)]
    [SerializeField] private float headbobFrequency = 10f;
    [SerializeField] private float headbobAmplitude = 0.05f;
    
    [Space(10f)]
    [SerializeField] private TestMovement player;
    [SerializeField] private Transform cameraRoot;
    
    [Space(10f)]
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    [SerializeField] private float lookSensitivity;
    
    [Space(10f)]
    [SerializeField] private Vector3 recoil;

    
    private CinemachineVirtualCamera _virtualCamera;
    
    private Vector3 _defaultLocalPos;
    private Vector3 _curRecoil;
    private Vector2 _mouseDelta;

    private Tween _zoomTween;
    
    private float _defaultFOV;
    private float _camCurXRot;


    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        _defaultFOV = _virtualCamera.m_Lens.FieldOfView;
    }

    private void Start()
    {
        _defaultLocalPos = cameraRoot.localPosition;
    }

    private void Update()
    {
        //UpdateHeadBob(true, testRatio);
        
        _mouseDelta = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        if (Input.GetMouseButton(0))
        {
            _curRecoil = new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
        }
    }

    private void LateUpdate()
    {
        CameraLook();

        _curRecoil = Vector3.zero;
    }

    public void SetFOV(float fov)
    {
        fov = Mathf.Clamp(fov, Constants.MinFOV, Constants.MaxFOV);

        _virtualCamera.m_Lens.FieldOfView = fov;

        _defaultFOV = fov;
    }


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
    
  
    public void UpdateHeadBob(bool isMove, float ratio)
    {
        float frequency = headbobFrequency * Mathf.Lerp(0.5f, 1.5f, ratio); 
        
        if (isMove)
        {
            float bobY = Mathf.Sin(Time.time * frequency) * headbobAmplitude;
            float bobX = Mathf.Cos(Time.time * frequency * 0.5f) * headbobAmplitude * 0.5f;
            
            cameraRoot.localPosition = _defaultLocalPos + new Vector3(bobX, bobY, 0);
        }
        else
        {
            cameraRoot.localPosition = Vector3.Lerp(cameraRoot.localPosition, _defaultLocalPos, Time.deltaTime * 5f);
        }
    }
    
    
    private void CameraLook()
    {
        var mouseDelta = new Vector3(_mouseDelta.x, _mouseDelta.y, 0);
        
        Vector3 totalInput = mouseDelta * lookSensitivity + _curRecoil; 
        
        _camCurXRot -= totalInput.x;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook); 
        
        cameraRoot.localRotation = Quaternion.Euler(_camCurXRot, 0f, 0f);

        player.transform.Rotate(Vector3.up * totalInput.y);
    }
    
}
