using System;
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
    [SerializeField] protected WeaponType weapontype;
    


    [SerializeField] protected GameObject DropObject;

    #region MuzzleEffect
    [SerializeField] protected GameObject muzzleFlash;
    [SerializeField] protected ParticleSystem muzzleSmoke;
    #endregion

    #region Sound & Animation
    [SerializeField] protected Animator WeaponAnimator;
    [SerializeField] protected GameObject playerArm;

    
    protected static readonly int IsAiming = Animator.StringToHash("IsAiming");
    protected static readonly int IsSprint = Animator.StringToHash("IsSprint");
    protected static readonly int ReLoadingTrigger = Animator.StringToHash("ReLoad");
    protected static readonly int AimFireTrigger = Animator.StringToHash("AimFire");
    protected static readonly int FireTrigger = Animator.StringToHash("Fire"); //일단 칼은 이거 그대로 가져가서 사용함

    #endregion
    protected bool isShootable;
    protected float lastFireTime;


    protected AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = AudioManager.Instance;
    }


    protected virtual void OnEnable()
    {
        AudioManager.Instance.PlayAudio(AudioID.WeaponSwap, 1f);
        PlayerEvent.OnAttack += Fire;
        PlayerEvent.OnSprint += SprintMove;
    }


    protected virtual void OnDisable()
    {
        PlayerEvent.OnAttack -= Fire;
        PlayerEvent.OnSprint -= SprintMove;
        StopAllCoroutines();
    }
    

    public virtual void Fire()
    {
        if (!GetFirerate())
        {
            isShootable = false;
        }
        else
        {
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


      

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }

    }

    protected void SprintMove(bool isSprint)
    {
        if (WeaponAnimator == null)
        {
            Debug.Log("달리기 애니메이션 없음");
            return;

        }
        WeaponAnimator.SetBool(IsSprint, isSprint);
    }


    public void DropItem()
    {
        Instantiate(DropObject, transform.position + transform.forward, Quaternion.identity);
        playerArm.SetActive(false);
        Destroy(this.gameObject);
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        if (muzzleFlash == null)
        {
            yield break;
        }
        muzzleFlash.SetActive(true);
        if (muzzleSmoke != null)
        {
            muzzleSmoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); //재시작을 위한 코드
            muzzleSmoke.Play();
        }
        yield return new WaitForSeconds(fireRate *0.5f);

        muzzleFlash.SetActive(false);
    }

    public WeaponType GetWeaponType()
    {
        return weapontype;
    }


    public void SwapWeaponOff()
    {
        playerArm.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void SwapWeaponON()
    {

        playerArm.SetActive(true);
        this.gameObject.SetActive(true);
    }

    public bool GetFirerate()
    {
        return Time.time >= lastFireTime + fireRate;  
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
