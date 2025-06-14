using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    private FPSVirtualCamera _fpsVirtualCamera;
    

    private void Awake()
    {
        _fpsVirtualCamera = FindAnyObjectByType<FPSVirtualCamera>();
    }
    
    public void PlayRecoilToFire(Vector3 recoil)
    {
        _fpsVirtualCamera.PlayRecoilToFire(recoil);
    }

    public void SetFOV(float value)
    {
        value = Mathf.Clamp(value, Constants.MinFOV, Constants.MaxFOV);
        
        _fpsVirtualCamera.SetFOV(value);
    }

    public void SetMouseSensitivity(float value)
    {
        value = Mathf.Clamp(value, Constants.MinMouseSensitivity, Constants.MaxMouseSensitivity);
        
        _fpsVirtualCamera.SetMouseSensitivity(value);
    }

    public void ZoomIn(float zoomValue, float duration = 0.5f)
    {
        _fpsVirtualCamera.ZoomIn(zoomValue, duration);
    }
    
    
    public void ZoomOut(float duration = 0.5f)
    {
        _fpsVirtualCamera.ZoomOut(duration);
    }
    
  
    public void UpdateHeadBob(float ratio)
    {
        _fpsVirtualCamera.UpdateHeadBob(ratio);
    }
    
}
