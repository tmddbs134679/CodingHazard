using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private bool canOnEnabled;

    [SerializeField] private float fadeDuration;
    
    [SerializeField] private float loopHoldDelay;

    [SerializeField] private Color fadeOutColor;


    private Image _image;

    private WaitForSeconds _returnHoldSec;

    private void Awake()
    {
        _image = GetComponent<Image>();

        _returnHoldSec = new WaitForSeconds(loopHoldDelay);
    }

    private void OnEnable()
    {
        if (canOnEnabled)
        {
            FadeOut(() => StartCoroutine(LoopCoroutine()));
        }
    }

    public void FadeIn()
    {
        Color fadeInColor = _image.color;

        fadeInColor.a = 0;
        
        _image.DOColor(fadeInColor, fadeDuration).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }

    public void FadeOut(UnityAction onCompleteCallback = null)
    {
        gameObject.SetActive(true);

        Color curColor = _image.color;

        curColor.a = 0;
        
        _image.color = curColor;
        
        _image.DOColor(fadeOutColor, fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                onCompleteCallback?.Invoke();
            });
    }

    IEnumerator LoopCoroutine()
    {
        yield return _returnHoldSec;

        FadeIn();
    }
}
