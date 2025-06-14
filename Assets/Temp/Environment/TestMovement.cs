using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 curMovementInput;

    private Rigidbody rigidbody;

    private CameraManager _cameraManager;

    [SerializeField] private float moveSpeed;

    [SerializeField] private FPSVirtualCamera fpsVirtualCamera;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _cameraManager = CameraManager.Instance;

        _cameraManager.SetMouseSensitivity(1);
        
        ToggleCursor(false);
    }

    private void Update()
    {
        curMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (Input.GetMouseButton(0))
        {
            _cameraManager.PlayRecoilToFire(Vector3.one);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            _cameraManager.ZoomIn(-20f);
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            _cameraManager.ZoomOut();
        }

        if (curMovementInput != Vector2.zero)
        {
            _cameraManager.UpdateHeadBob(1);
        }
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        curMovementInput.Normalize();
        
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = toggle;
    }
}
