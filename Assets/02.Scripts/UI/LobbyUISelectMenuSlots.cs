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

    private AudioManager _audioManager;
    
    
    public void Init(LobbyUI rootUI, LobbyUISelectMenu rootMenu)
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        
        _button = GetComponent<Button>();

        _rootUI = rootUI;
        
        _button.onClick.AddListener(() =>
        {
            rootMenu.Hide();
            
            _audioManager.PlayAudio(AudioID.ClickUI, 0.1f);
            
        });

        _audioManager = AudioManager.Instance;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _textMeshProUGUI.color = _rootUI.PointerEnterColor;
        
        _audioManager.PlayAudio(AudioID.EnterUI, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _textMeshProUGUI.color = _rootUI.PointerExitColor;
    }
}
