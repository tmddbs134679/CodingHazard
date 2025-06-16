using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        if (Input.GetMouseButton(0))
        {
            fpsVirtualCamera.PlayRecoilToFire(Vector3.one * 0.5f);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            fpsVirtualCamera.ZoomIn(-20f, 0.5f);
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            fpsVirtualCamera.ZoomOut(0.5f);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Physics.Raycast(fpsVirtualCamera.transform.position, fpsVirtualCamera.transform.forward, out RaycastHit hit, 1000f);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out DroppedItem item))
                {
                    item.OnInteract();
                }
            }
        }

        if (curMovementInput != Vector2.zero)
        {
            fpsVirtualCamera.UpdateHeadBob(1);
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
