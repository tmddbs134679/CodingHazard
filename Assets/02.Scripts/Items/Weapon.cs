using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [field: Header("WeaponInfo")]
    [SerializeField] protected string WeaponName;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float range = 100f;
    [SerializeField] protected float fireRate = 0.1f;

    [SerializeField] protected Animator WeaponAnimator;

    [SerializeField] protected GameObject DropObject;

    protected static readonly int IsAming = Animator.StringToHash("IsAiming");
    protected static readonly int IsMoving = Animator.StringToHash("IsMoving");
    protected static readonly int IsFire = Animator.StringToHash("IsFire");

    protected bool isShootable;
    protected float lastFireTime;
    public virtual void Fire()
    {
        if (Time.time < lastFireTime + fireRate)
        {
            isShootable = false;
        }
        else
        {

            isShootable = true;
            lastFireTime = Time.time;
        }



        //이펙트 및 사운드도 나중에 추가


        //여기서 판단하면 안될듯 하다. 

    }


    protected void Start()
    {

    }
    protected void Update()
    {
        //test
        if (Input.GetMouseButton(0))
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }
        
    }

    protected void DropItem()
    {
        Instantiate(DropObject, transform.position + transform.forward ,Quaternion.identity);
        Destroy(this.gameObject);
    }

   

}
