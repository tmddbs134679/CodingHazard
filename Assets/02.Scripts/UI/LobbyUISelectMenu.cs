using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUISelectMenu : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private LobbyUISelectMenuSlots[] _slots;
    private LobbyUI _rootUI;
    
    
    public void Init(LobbyUI rootUI)
    {
        _rootUI = rootUI;
        
        _canvasGroup = GetComponent<CanvasGroup>();
        
        _slots = GetComponentsInChildren<LobbyUISelectMenuSlots>();

        foreach (var slot in _slots)
        {
            slot.Init(rootUI, this);
        }
    }

    public void Show()
    {
        DOTween.To(()=> _canvasGroup.alpha, a=> _canvasGroup.alpha = a, 1, 0.33f).
            OnComplete(()=>_canvasGroup.blocksRaycasts = true);
        
        transform.DOMove(_rootUI.MenuShowPoint.position,0.33f);
        
        _rootUI.EnabledMenus.Push(this);
    }

    public void Hide()
    {
        DOTween.To(()=> _canvasGroup.alpha, a=> _canvasGroup.alpha = a, 0, 0.33f).
            OnComplete(() => transform.position = _rootUI.MenuHidePoint.position);

        _canvasGroup.blocksRaycasts = false;
    }
}
