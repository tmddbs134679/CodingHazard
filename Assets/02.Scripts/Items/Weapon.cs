using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum WeaponType
{
    Gun,
    Knife
}
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

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip audioClip;
    [SerializeField] protected GameObject playerArm;
    [SerializeField] protected WeaponType weapontype;

    //Aciont

    protected static readonly int IsAiming = Animator.StringToHash("IsAiming");
    //protected static readonly int IsMoving = Animator.StringToHash("IsMoving");
    protected static readonly int AimFireTrigger = Animator.StringToHash("AimFire");
    protected static readonly int FireTrigger = Animator.StringToHash("Fire");

    protected bool isShootable;
    protected float lastFireTime;

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void OnEnable()
    {

    }
    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }


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

    public void Damage()
    {

    }



    protected void DropItem()
    {
        Instantiate(DropObject, transform.position + transform.forward, Quaternion.identity);
        playerArm.SetActive(false);
        Destroy(this.gameObject);
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(fireRate * 1.2f);

        muzzleFlash.SetActive(false);
    }



    protected void PlayAttackAnimation(bool isAiming)
    {
        if (WeaponAnimator == null)
        {
            return;
        }

        switch (weapontype)
        {
            case WeaponType.Gun:
                if (isAiming)
                {
                    WeaponAnimator.SetTrigger(AimFireTrigger);
                }
                else
                {
                    WeaponAnimator.SetTrigger(FireTrigger);
                }
                break;

            case WeaponType.Knife:
                WeaponAnimator.SetTrigger(FireTrigger);
                break;
        }
    }
}
