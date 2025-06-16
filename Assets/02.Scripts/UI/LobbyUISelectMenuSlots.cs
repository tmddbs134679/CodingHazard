using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyUISelectMenuSlots : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI _textMeshProUGUI;

    private LobbyUI _rootUI;

    private Button _button;
    
    public void Init(LobbyUI rootUI, LobbyUISelectMenu rootMenu)
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        
        _button = GetComponent<Button>();

        _rootUI = rootUI;
        
        _button.onClick.AddListener(rootMenu.Hide);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _textMeshProUGUI.color = _rootUI.PointerEnterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _textMeshProUGUI.color = _rootUI.PointerExitColor;
    }
}
