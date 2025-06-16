using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image loadingImage;
    
    private string[] _loadingTexts = new[]
    {
        "Loading.",
        "Loading..",
        "Loading...",
        "Loading....",
    };


    private SceneLoadManager _sceneLoadManager;
    
    
    private void Start()
    {
        _sceneLoadManager = SceneLoadManager.Instance;
        
        StartCoroutine(LoadingTextCoroutine());
    }

   

    IEnumerator LoadingTextCoroutine()
    {
        while (true)
        {
            foreach (var text in _loadingTexts)
            {
                loadingText.text = text;
                
                loadingImage.fillAmount = _sceneLoadManager.CurProgressRatio;

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
