using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CrossHair : UI_Base
{
    enum GameObjects
    {
        CrossHair,
        AimHair,
        DamageCrossHair,
        KillEffectCrossHair

    }
    private Vector3 _originalScale;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        #endregion


        return true;
    }

    private void Awake()
    {
        Init();
        _originalScale = GetObject((int)GameObjects.AimHair).GetComponent<RectTransform>().localScale;
        GetObject((int)GameObjects.DamageCrossHair).SetActive(false);
        GetObject((int)GameObjects.KillEffectCrossHair).SetActive(false);
    }

    public float recoilScaleAmount = 1.3f;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            AnimateRecoilCrosshair(GetObject((int)GameObjects.AimHair));
        }
        else if(Input.GetMouseButtonDown(1))
        {
            OnAimStart();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            GetObject((int)GameObjects.DamageCrossHair).SetActive(true);
            AnimateRecoilCrosshair(GetObject((int)GameObjects.DamageCrossHair));
            AutoHideAfterDelay(GetObject((int)GameObjects.DamageCrossHair));
        }
    }

    private const float tweenDuration = 0.1f;

    //총알이 발사될 때 실행
    private void AnimateRecoilCrosshair(GameObject crossHair)
    {
        crossHair.transform.DOKill();
        crossHair.transform.localScale = _originalScale;

        Sequence seq = DOTween.Sequence();
        Vector3 Objectscale = crossHair.GetComponent<RectTransform>().localScale;
        seq.Append(crossHair.transform.DOScale(Objectscale * recoilScaleAmount, tweenDuration).SetEase(Ease.OutQuad));
        seq.Append(crossHair.transform.DOScale(Objectscale, tweenDuration).SetEase(Ease.InQuad));
    }

    public void OnAimStart()
    {
        GetObject((int)GameObjects.CrossHair).SetActive(false);
    }

    public void OnAimEnd()
    {
        GetObject((int)GameObjects.CrossHair).SetActive(true);
    }

    private void AutoHideAfterDelay(GameObject crossHair, float delay = 0.2f)
    {
        crossHair.transform.DOKill(); 

        // 활성화 보장
        if (!crossHair.activeSelf)
            crossHair.SetActive(true);

        crossHair.transform
            .DOScale(_originalScale, 0) 
            .SetDelay(delay)
            .OnComplete(() => crossHair.SetActive(false));
    }
}
