using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StaminaBar : UI_Base
{
    private const float clampratio = 0.35f; //0.35 ~ 0 을 1부터 0비율로 맞추는 값
    enum Images
    {
        StaminaDamageBar,
        StaminaBar,
    }
    void OnEnable()
    {
       PlayerEvent.OnStaminaChanged += SetStaminaRatio;
    }

    public void OnDisable()
    {
       PlayerEvent.OnStaminaChanged -= SetStaminaRatio;
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindImage(typeof(Images));
        return true;
    }

    float _lastFillAmount = -1;
    public void SetStaminaRatio(float curvalue)
    {
        if (!_init) return;

        float max = StageManager.Instance.PlayerController.GetComponent<PlayerCondition>().stamina.maxValue;
        float ratio = Mathf.Clamp01(curvalue / max);
        float scaledRatio = ratio * clampratio;

        Image bar = GetImage((int)Images.StaminaBar);
        Image dmgBar = GetImage((int)Images.StaminaDamageBar);

        // 1. 즉시 갱신되는 주 바
        bar.fillAmount = scaledRatio;

        // 2. 감소 or 회복 판단
        if (_lastFillAmount > scaledRatio)
        {
            // 감소 중이면 Tween 사용 (천천히 따라오게)
            DOTween.Kill(dmgBar);
            dmgBar.DOFillAmount(scaledRatio, 1f).SetEase(Ease.OutQuad);
        }
        else
        {
            // 회복 중이면 바로 채워버림
            dmgBar.fillAmount = scaledRatio;
        }

        _lastFillAmount = scaledRatio;
    }

}
