using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_HPBar : UI_Base
{
    private const float clampratio = 0.35f; //0.35 ~ 0 �� 1���� 0������ ���ߴ� ��
    enum Images
    {
        HPDamageBar,
        HPBar,
    }
    public void OnEnable()
    {
        PlayerEvent.OnHpChanged += SetHpRatio;
    }

    public void OnDisable()
    {
        PlayerEvent.OnHpChanged -= SetHpRatio;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindImage(typeof(Images));
        return true;
    }
    public void SetHpRatio(float hp)
    {
        //float maxHp = 100f;
        float ratio = Mathf.Clamp01(hp / StageManager.Instance.PlayerController.GetComponent<PlayerCondition>().hp.maxValue);      
        float scaledRatio = ratio * clampratio;
        GetImage((int)Images.HPBar).fillAmount = scaledRatio;

        DOTween.Kill(GetImage((int)Images.HPDamageBar));

        GetImage((int)Images.HPDamageBar).DOFillAmount(scaledRatio, 1f).SetEase(Ease.OutQuad);
    }

   
}
