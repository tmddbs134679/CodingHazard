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

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindImage(typeof(Images));
        return true;
    }
    public void SetStaminaRatio(ref float hp)
    {
        hp -= 20;
        float maxHp = 100f;
        float ratio = Mathf.Clamp01(hp / maxHp);
        float scaledRatio = ratio * clampratio;
        GetImage((int)Images.StaminaBar).fillAmount = scaledRatio;

        DOTween.Kill(GetImage((int)Images.StaminaDamageBar));

        GetImage((int)Images.StaminaDamageBar).DOFillAmount(scaledRatio, 1f).SetEase(Ease.OutQuad);
    }

}
