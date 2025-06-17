using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gun : Weapon
{
    public enum FireMode { Single, Auto}

    [SerializeField] private FireMode fireMode;
    [SerializeField] private Transform firePoint;


    [Header("ReCoil")]
    [SerializeField] private Transform curPos;
    [SerializeField] private float recoilx = 0.01f;
    [SerializeField] private float recoily = 0.01f;
    [SerializeField] private float recoilz = 0.05f;


    [SerializeField] private int Ammo { get; set; }

    [Header("Camera")]
    [SerializeField] private float normalFOV = 50f;
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

      //  HandleFireInput();


        if (Input.GetMouseButtonDown(1))
        {
            //이벤트 연결하기 
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
        StartCoroutine("OnMuzzleFlashEffect");
        if (isShootable == false)
        {
            return;
        }

        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        PlaySound(audioClip);

        PlayAttackAnimation(isZoom);
         
       LayerMask layerMask = 1<<9;
        if (Physics.Raycast(ray, out RaycastHit hit, range,layerMask))
        {

            Debug.Log("맞음");
           
            if (hit.collider.TryGetComponent<HitBox>(out var enemy))
            {
                
                    //나중에 변경될 수 있음 
                    enemy.Damaged(damage,hit);
             
                
            }
          
        }
        StartCoroutine(ApplyRecoil());
        
            
    }

    private IEnumerator ApplyRecoil()
    {
        if (curPos == null)
        {
            yield break;
        }

        Vector3 recoil = new Vector3(Random.Range(-recoilx, recoilx), Random.Range(-recoily, recoily), -recoilz);
        Vector3 originPos = curPos.localPosition;
        Vector3 targetPos = originPos + recoil;

        float time = 0f;
        float recoilSpeed = 20f;

        while (time < 1f)
        {
            time += Time.deltaTime * recoilSpeed;
            curPos.localPosition = Vector3.Lerp(targetPos, originPos, time);
            yield return null;
        }

    }

    


}
