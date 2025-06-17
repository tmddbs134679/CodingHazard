using System;
using System.Collections.Generic;
using JetBrains.Annotations;
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

    [SerializeField] private GameObject gameVolumesRoot;
    [SerializeField] private GameObject lobbyVolumesRoot;

    private Volume[] _gameVolumes;

    private void Awake()
    {
        _gameVolumes = gameVolumesRoot.transform.GetComponentsInChildren<Volume>();
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
        if (TryGetVolumeComponents(out List<T> components))
        {
            foreach (var component in components)
            {
                component.active = state;
            }
        }
    }

    public bool TryGetVolumeComponent<T>(out T targetComponent) where T : VolumeComponent
    {
        foreach (var volume in _gameVolumes)
        {
            if (volume.profile.TryGet(out T component))
            {
                targetComponent = component;
                return true;
            }
        }
        targetComponent = null;
        return false;
    }
    
    public bool TryGetVolumeComponents<T>(out List<T> targetComponent) where T : VolumeComponent
    {
        targetComponent = new();
        
        foreach (var volume in _gameVolumes)
        {
            if (volume.profile.TryGet(out T component))
            {
                targetComponent.Add(component);
                return true;
            }
        }
        
        return false;
    }
}
