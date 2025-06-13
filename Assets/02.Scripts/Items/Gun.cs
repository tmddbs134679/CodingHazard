using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gun : Weapon
{
    public enum FireMode { Single, Auto}

    [SerializeField] private FireMode fireMode;
    [SerializeField] private Transform firePoint;
   

    [Header("Weapon Pos")]
    [SerializeField] private Transform weaponPos;
    [SerializeField] private Transform normalPos;
    [SerializeField] private Vector3 zoomPos;


    [Header("Camera")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float zoomFOV = 40;
    [SerializeField] private float zoomSpeed = 10f;
   


    private Camera mainCam;
    private bool isZoom = false;


    protected void Start()
    {
        mainCam = Camera.main;


    }

    protected void Update()
    {

        base.Update();

        HandleFireInput();


        if (Input.GetMouseButtonDown(1))
        {
            isZoom = !isZoom;
            ZoomWeapon();
        }


        //업데이트에서 하면 너무 많이 입력됨
       
    }

    private void ZoomWeapon()
    {
      //  weaponPos.localPosition = Vector3.Lerp(weaponPos.localPosition, targetPos, Time.deltaTime);
        WeaponAnimator.SetBool(IsAiming, isZoom);
        float targetFOV = isZoom ? zoomFOV : normalFOV;
        if (mainCam != null)
        {
            mainCam.fieldOfView = targetFOV;
        }

    }


    private void HandleFireInput()
    {
        switch (fireMode)
        {
            case FireMode.Single:
                if (Input.GetMouseButtonDown(0))
                    Fire();
                break;
            case FireMode.Auto:
                if (Input.GetMouseButton(0))
                    Fire();
                break;
        }
    }

    public override void Fire()
    {
        base.Fire();
        if (isShootable == false)
        {
            return;
        }
       
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        Debug.DrawRay(firePoint.position, firePoint.forward* range, Color.red); //나중에 제거 예정
        PlaySound(audioClip);
        if (isZoom == false)
        {
            WeaponAnimator.SetTrigger(FireTrigger);
        }
        else if (isZoom == true)
        {
            WeaponAnimator.SetTrigger(AimFireTrigger);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {

            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log(hit.collider.gameObject.name);
            }

            /*
             좀비 확장  벽이면 패스 이렇게 할둣
            if (hit.collider.TryGetComponent(Monster))
            { 
                
            }
            */

        }
        
            
    }

    


}
