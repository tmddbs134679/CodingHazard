using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FPSVirtualCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetFOV(float fov, float duration = 0)
    {
        fov = Mathf.Clamp(fov, Constants.MinFOV, Constants.MaxFOV);

        if (duration > 0)
        {
            _virtualCamera.m_Lens.FieldOfView = fov;
        }
        else
        {
            float currentFov = _virtualCamera.m_Lens.FieldOfView;
            
            /*DOTween.To(
                () => virtualCamera.m_Lens.FieldOfView,
                x => virtualCamera.m_Lens.FieldOfView = x,
                targetFOV,
                duration*/
                
        }
    }
}
