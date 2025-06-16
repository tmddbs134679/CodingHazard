using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StaminaBar : UI_Base
{
    private const float clampratio = 0.35f; //0.35 ~ 0 을 1부터 0비율로 맞추는 값
    enum Images
    {
        StaminaDamageBar,
        StaminaBar,
    }
    public void OnEnable()
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

    public void SetStaminaRatio(float curvalue)
    {
        //스테미나 확실하게 ㅁ어케 작동하는지 모르겠음
        float ratio = Mathf.Clamp01(curvalue / StageManager.Instance.PlayerController.GetComponent<PlayerCondition>().stamina.maxValue);
        float scaledRatio = ratio * clampratio;
        GetImage((int)Images.StaminaBar).fillAmount = scaledRatio;

        DOTween.Kill(GetImage((int)Images.StaminaDamageBar));

        GetImage((int)Images.StaminaDamageBar).DOFillAmount(scaledRatio, 1f).SetEase(Ease.OutQuad);
    }

}
