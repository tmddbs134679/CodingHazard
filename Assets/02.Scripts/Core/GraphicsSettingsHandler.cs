using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraphicsSettingsHandler : MonoBehaviour
{
    [SerializeField] private VolumeProfile postProcessProfile;
    [SerializeField] private FPSVirtualCamera fpsVirtualCamera;

    private UniversalAdditionalCameraData _urpCameraData;

    private void Start()
    {
        _urpCameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
    }

    public void SetFov(float value)
    {
        value = Mathf.Clamp(value, Constants.MinFOV, Constants.MaxFOV);

        fpsVirtualCamera.SetFOV(value);
    }
    
    public void SetMouseSensitivity(float value)
    {
        value = Mathf.Clamp(value, Constants.MinMouseSensitivity, Constants.MaxMouseSensitivity);

        fpsVirtualCamera.SetLookSensitivity(value);
    }
    
    public void SetAntiAliasing(AntialiasingMode mode)
    {
        if (_urpCameraData != null)
        {
            _urpCameraData.antialiasing = mode;
        }
    }
   
    public void TogglePostProcessEffect<T>(bool state) where T : VolumeComponent
    {
        if (postProcessProfile.TryGet(out T effect))
        {
            effect.active = state;
        }
    }
}
