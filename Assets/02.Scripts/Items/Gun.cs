using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Transform firePoint;
   

    [Header("Weapon Pos")]
    [SerializeField] private Transform weaponPos;
    [SerializeField] private Vector3 normalPos;
    [SerializeField] private Vector3 zoomPos;



    private Camera mainCam;
    private bool isZoom = false;


    protected void Start()
    {
      
        mainCam = Camera.main;
        if (weaponPos != null)
        { 
        weaponPos.localPosition = normalPos;
        }
    }

    protected void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(1))
        {
            isZoom = !isZoom;
            ZoomWeapon();
        }


        //업데이트에서 하면 너무 많이 입력됨
       
    }

    private void ZoomWeapon()
    {
        if (weaponPos == null) return;

        Vector3 targetPos = isZoom ? zoomPos : normalPos;
        weaponPos.localPosition = Vector3.Lerp(weaponPos.localPosition, targetPos, Time.deltaTime);
        WeaponAnimator.SetBool(IsAming, isZoom);

    }

    public override void Fire()
    {
        base.Fire();
        if (isShootable == false)
        {
            return;
        }
        //부모에서 딜레이가 되면 그냥 리턴되고 연사를 구현할때 다시 봐야할듯하다. 
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        Debug.DrawRay(firePoint.position, firePoint.forward* range, Color.red); //나중에 제거 예정

        WeaponAnimator.SetTrigger(IsFire);
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
