using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameSettingManager : MonoBehaviour
{
    public event UnityAction<float> OnChangedFOV;
    public event UnityAction<float> OnChangedMouseSensitivity;
    public event UnityAction<AntialiasingMode> OnChangedAntialiasingMode;
    
    public float FOV { get; private set; }
    public float MouseSensitivity { get; private set; }
    public AntialiasingMode AntialiasingMode { get; private set; }

    
    [SerializeField] private GameSettings baseSettings;
    
    
    private Volume _volume;
    
    public void Init()
    {
        _volume = GetComponent<Volume>();
    }

    public void SetBaseState()
    {
        SetFov(baseSettings.fov);
        SetMouseSensitivity(baseSettings.mouseSensitivity);
        SetAntiAliasingMode(baseSettings.antialiasing);
    }

    public void SetFov(float value)
    {
        value = Mathf.Clamp(value, Constants.MinFOV, Constants.MaxFOV);

        FOV = value;
        
        OnChangedFOV?.Invoke(FOV);
    } 
    
    public void SetMouseSensitivity(float value)
    {
        value = Mathf.Clamp(value, Constants.MinMouseSensitivity, Constants.MaxMouseSensitivity);

        MouseSensitivity = value;
        
        OnChangedMouseSensitivity?.Invoke(MouseSensitivity);
    }
    
    public void SetAntiAliasingMode(AntialiasingMode mode)
    {
        AntialiasingMode = mode;
        
        OnChangedAntialiasingMode?.Invoke(AntialiasingMode);
    }
   
    public void ToggleVolumeComponent<T>(bool state) where T : VolumeComponent
    {
        if (TryGetVolumeComponent(out T component))
        {
            component.active = state;
        }
    }

    public bool TryGetVolumeComponent<T>(out T targetComponent) where T : VolumeComponent
    {
        if (_volume.profile.TryGet(out T component))
        {
            targetComponent = component;
            return true;
        }

        targetComponent = null;
        return false;
    }
}
