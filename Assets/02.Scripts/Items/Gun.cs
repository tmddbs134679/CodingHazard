using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public enum FireMode { Single, Auto }

    public int CuAmmo { get { return curAmmo; } }
    public int MaxAmmo { get { return maxAmmo; } }
    public int SpareAmmo { get { return spareAmmo; } }

    [Header("Ray")]
    [SerializeField] private float spreadWidth; //가로 
    [SerializeField] private float spreadHeight;  //세로


    [SerializeField] private FireMode fireMode;
    [SerializeField] private Transform firePoint;

    [Header("AMMMO")]
    [SerializeField] private int curAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int spareAmmo;


    [Header("ReCoil")]
    [SerializeField] private float recoilx = 0.01f;
    [SerializeField] private float recoily = 0.01f;
    [SerializeField] private float recoilz = 0.05f;


    [SerializeField] private int Ammo { get; set; }

    [Header("Camera")]
    [SerializeField] private float normalFOV = 50f;
    [SerializeField] private float zoomFOV = 40;
    [SerializeField] private float zoomSpeed = 10f;

    [Header("Audio")]
    [SerializeField] private AudioID audioID;
    [SerializeField] private GameObject bulletDecalPrefab;
    [SerializeField] private LayerMask hitLayerMask;


    private Camera mainCam;
    private bool isZoom = false;


    protected void Start()
    {
        mainCam = Camera.main;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerEvent.Aiming += ZoomWeapon;
        PlayerEvent.OnReLoad += ReLoading;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerEvent.Aiming -= ZoomWeapon;
        PlayerEvent.OnReLoad -= ReLoading;
    }

    protected void Update()
    {
        base.Update();
    }
    

    private void SpawnBulletDecal(RaycastHit hit)
    {
        Vector3 position = hit.point + hit.normal * 0.01f; 
        Quaternion rotation = Quaternion.LookRotation(hit.normal * -1f); 

        Instantiate(bulletDecalPrefab, position, rotation);
    }

    private void ReLoading()
    {
        WeaponAnimator.SetTrigger(ReLoadingTrigger);

        if (curAmmo < maxAmmo && spareAmmo > 0)
        {
            int neededAmmo = maxAmmo - curAmmo;


            if (spareAmmo >= neededAmmo)
            {
                curAmmo += neededAmmo;
                spareAmmo -= neededAmmo;
            }

            else
            {
                curAmmo += spareAmmo;
                spareAmmo = 0;
            }
        }

        PlayerEvent.OnUpdateBullet?.Invoke(spareAmmo, curAmmo);
    }

    private void ZoomWeapon(bool isZoom)
    {
        this.isZoom = isZoom;
        WeaponAnimator.SetBool(IsAiming, isZoom);

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

        if (curAmmo <= 0)
        {
            return;
        }
        base.Fire();
        StartCoroutine("OnMuzzleFlashEffect");

        Vector3 veiwport;

        if (isShootable == false)
        {
            return;
        }
        if (isZoom)
        {
            veiwport = new Vector3(0.5f, 0.5f, 0f); //정가운데
        }
        else
        {
            float halfW = spreadWidth * 0.5f;
            float halfH = spreadHeight * 0.5f;
            float randX = Random.Range(0.5f - halfW, 0.5f + halfW);
            float randY = Random.Range(0.5f - halfH, 0.5f + halfH);
            veiwport = new Vector3(randX, randY, 0f);
           
        }

        Ray ray = mainCam.ViewportPointToRay(veiwport);

        
        _audioManager.PlayAudio(audioID, 0.10f);
        

        PlayAttackAnimation(isZoom);

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayerMask))
        {
            if (hit.collider.TryGetComponent<HitBox>(out var enemy))
            {
                //나중에 변경될 수 있음 
                enemy.Damaged(damage, hit);
            }
            else
            {
                SpawnBulletDecal(hit);
            }
        }
        StartCoroutine(ApplyRecoil());
        curAmmo--;
        PlayerEvent.OnUpdateBullet?.Invoke(spareAmmo, curAmmo);
    }

    private IEnumerator ApplyRecoil()
    {


        Vector3 recoil = new Vector3(Random.Range(-recoilx, recoilx), Random.Range(-recoily, recoily), -recoilz);
        Vector3 originPos = this.transform.localPosition;
        Vector3 targetPos = originPos + recoil;

        float time = 0f;
        float recoilSpeed = 20f;

        while (time < 1f)
        {
            time += Time.deltaTime * recoilSpeed;
            this.transform.localPosition = Vector3.Lerp(targetPos, originPos, time);
            yield return null;
        }

    }



    //탄약 먹으면 증가해야하잖아
    public void AddSpareAmmo(int count)
    {
        spareAmmo += count;
    }




}
