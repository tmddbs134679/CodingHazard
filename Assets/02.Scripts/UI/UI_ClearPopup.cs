using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI_ClearPopup : UI_Base
{
    #region Enum
    enum GameObjects
    {
        TextObject,
    }
    enum Texts
    {
        ToastPopupText,
        TeamNameText,
        ButtonText,
    }
    enum Buttons
    {
        TouchScreen,
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
        BindButton(typeof(Buttons));
        #endregion


        return true;
    }

    private void Awake()
    {
        Init();
        GetImage((int)Images.background).gameObject.SetActive(false);
        GetButton((int)Buttons.TouchScreen).gameObject.SetActive(false);
        #region Color √ ±‚»≠

        Color c = GetText((int)Texts.ToastPopupText).color;
        c.a = 0;
        GetText((int)Texts.ToastPopupText).color = c;
        GetText((int)Texts.ToastPopupText).gameObject.SetActive(false);

        #endregion

        GetButton((int)Buttons.TouchScreen).gameObject.BindEvent(OnClickScreenButton);
    }

    private void OnClickScreenButton()
    {
        SceneManager.LoadScene(1);
    }

    public IEnumerator StartCreditScrollCoroutine()
    {
        var rt = GetObject((int)GameObjects.TextObject).GetComponent<RectTransform>();
        float endpos = rt.anchoredPosition.y + 1900;

        yield return rt.DOAnchorPosY(endpos, 15f)
                       .SetEase(Ease.InOutSine)
                       .WaitForCompletion();
    }


    public float fadeTime = 5f;
    public float showTime = 5f;
    public IEnumerator ToastCoroutine()
    {
        var txt = GetText((int)Texts.ToastPopupText);
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

    public IEnumerator ButtonActive()
    {

        yield return GetObject((int)GameObjects.TextObject).GetComponent<CanvasGroup>().DOFade(0f, 1f).SetEase(Ease.OutQuad).WaitForCompletion();

        //GetButton((int)Buttons.TouchScreen).gameObject.SetActive(true);
        //yield return GetText((int)Texts.ButtonText).DOFade(1f, 1f).SetEase(Ease.InQuad).WaitForCompletion();

        SceneManager.LoadScene(1);
    }


}
