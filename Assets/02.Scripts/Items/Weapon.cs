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

    [SerializeField] protected GameObject muzzleFlash;

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

            StartCoroutine("OnMuzzleFlashEffect");
            isShootable = true;
            lastFireTime = Time.time;
        }


        //사운드도 나중에 추가
        //디테일 맞추면서 하는것도 나중에 



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
        Instantiate(DropObject, transform.position + transform.forward, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(fireRate * 1.2f);

        muzzleFlash.SetActive(false);
    }

}
