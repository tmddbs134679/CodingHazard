using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private bool canOnEnabled;

    [SerializeField] private float fadeDuration;
    
    [SerializeField] private float returnHoldTime;

    [SerializeField] private Color fadeOutColor;


    private Image _image;

    private WaitForSeconds _returnHoldSec;

    private void Awake()
    {
        _image = GetComponent<Image>();

        _returnHoldSec = new WaitForSeconds(returnHoldTime);
    }

    private void OnEnable()
    {
        if (canOnEnabled)
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        
        Color fadeInColor = _image.color;

        fadeInColor.a = 0;
        
        _image.DOColor(fadeInColor, fadeDuration).OnComplete(() => gameObject.SetActive(false));
    }

    private void FadeOut()
    {
        gameObject.SetActive(true);

        Color curColor = _image.color;

        curColor.a = 0;
        
        _image.color = curColor;
        
        _image.DOColor(fadeOutColor, fadeDuration).OnComplete(() =>
            {
                if (returnHoldTime > 0)
                {
                    StartCoroutine(ReturnHoldDelay());
                }
            });
    }

    IEnumerator ReturnHoldDelay()
    {
        yield return _returnHoldSec;

        FadeIn();
    }
}
