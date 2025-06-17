using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class UI_ClearPopup : UI_Base
{
    enum GameObjects
    {
        TextObject,
    }
    enum Texts
    {
        ToastPopupText,
        TeamNameText
    }

    enum Images
    {
        background,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        #endregion


        return true;
    }

    private void Awake()
    {
        Init();
        GetImage((int)Images.background).gameObject.SetActive(false);

        #region Color 초기화

        Color c = GetText((int)Texts.ToastPopupText).color;
        c.a = 0;
        GetText((int)Texts.ToastPopupText).color = c;
        GetText((int)Texts.ToastPopupText).gameObject.SetActive(false);

        #endregion
    }

    private void Start()
    {
        ToastObject();
        FadeIn();
        StartCreditScroll();
    }
    private void StartCreditScroll()
    {
        float endpos = GetObject((int)GameObjects.TextObject).GetComponent<RectTransform>().anchoredPosition.y + 2200;
        GetObject((int)GameObjects.TextObject).GetComponent<RectTransform>().DOAnchorPosY(endpos, 20f).SetEase(Ease.InOutSine);
    }



    public float fadeTime = 5f;
    public float showTime = 5f;
    private void ToastObject()
    {
        GetText((int)Texts.ToastPopupText).gameObject.SetActive(true);
        // Fade In → 유지 → Fade Out
        Sequence seq = DOTween.Sequence();

        seq.Append(GetText((int)Texts.ToastPopupText).DOFade(1f, fadeTime))      
           .AppendInterval(showTime)                 
           .Append(GetText((int)Texts.ToastPopupText).DOFade(0f, fadeTime));    
    }

    private const float CONST_FADETIME = 5f;
    private void FadeIn()
    {
        GetImage((int)Images.background).gameObject.SetActive(true);
        GetImage((int)Images.background).DOFade(1f, CONST_FADETIME).SetEase(Ease.Linear);
    }
}
