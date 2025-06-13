using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 curMovementInput;

    private Rigidbody rigidbody;

    [SerializeField] private float moveSpeed;

    [SerializeField] private FPSVirtualCamera fpsVirtualCamera;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        ToggleCursor(false);
    }

    private void Update()
    {
        curMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButtonDown(1))
        {
            fpsVirtualCamera.ZoomIn(-20);
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            fpsVirtualCamera.ZoomOut();
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
