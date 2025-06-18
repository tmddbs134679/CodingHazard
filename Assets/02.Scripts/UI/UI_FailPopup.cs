using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FailPopup : UI_Base
{
    #region Enum
    enum GameObjects
    {
        TextObject,
    }
    enum Texts
    {
        FailToastPopupText,
    }

    enum Images
    {
        background,
    }

    #endregion



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

        #region Color √ ±‚»≠

        Color c = GetText((int)Texts.FailToastPopupText).color;
        c.a = 0;
        GetText((int)Texts.FailToastPopupText).color = c;
        GetText((int)Texts.FailToastPopupText).gameObject.SetActive(false);

        #endregion
    }


    public float fadeTime = 2f;
    public float showTime = 2f;
    public IEnumerator ToastCoroutine()
    {
        var txt = GetText((int)Texts.FailToastPopupText);
        txt.gameObject.SetActive(true);

        yield return txt.DOFade(1f, fadeTime).WaitForCompletion();
        yield return new WaitForSeconds(showTime);
        yield return txt.DOFade(0f, fadeTime - 2).WaitForCompletion();
    }


    public IEnumerator FadeInCoroutine()
    {
        GetImage((int)Images.background).gameObject.SetActive(true);
        yield return GetImage((int)Images.background)
            .DOFade(1f, Constants.CONST_FADETIME)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }

}
