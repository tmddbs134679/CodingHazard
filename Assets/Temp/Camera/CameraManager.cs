using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera Camera
    {
        get
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }

            return _camera;
        }
    }

    private Camera _camera;


    private float _minFOV = 80;
    private float _maxFOV = 103;
    

    public void SetFOV(float value)
    {
        value = Mathf.Clamp(value, _minFOV, _maxFOV);
    }
}
