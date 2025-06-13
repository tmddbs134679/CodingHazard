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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _virtualCamera.m_Lens.FieldOfView = 70;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _virtualCamera.m_Lens.FieldOfView = 80;
        }
    }

    public void SetFOV(float fo)
    {
        
    }
}
