using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EditSetting : UI_Base
{

    #region Enum

    enum GameObjects
    {
        MenuToggleGroup,
        CheckDisplaymarkObject,
        CheckAudiomarkObject,

    }
    enum Buttons
    {

    }
    enum Texts
    {

    }
    enum Toggles
    {
        DisplayToggle,
        AudioToggle,
    }
    #endregion

    //�ӽ� ����
    [SerializeField] private UI_AudioPopup _AudioPopup;
    [SerializeField] private UI_DisplayPopup _DisplayPopup;

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        StageManager.Instance.PlayerController.BlockInput();

        Time.timeScale = 0;
        
        _DisplayPopup.gameObject.SetActive(true);
        AudioManager.Instance.PlayAudio(AudioID.EnterUI, 0.5f);
    }

    private void OnDisable()
    {
        if (_DisplayPopup == null || _AudioPopup == null) return;
        
        _DisplayPopup.gameObject.SetActive(false);
        _AudioPopup.gameObject.SetActive(false);
        
        Time.timeScale = 1;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        StageManager.Instance.PlayerController.UnblockInput();
    }
    public override bool Init()
    {
        #region Object Bind

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindToggle(typeof(Toggles));
  

        // ��� Ŭ�� �� �ൿ
        GetToggle((int)Toggles.DisplayToggle).gameObject.BindEvent(OnClickDisplayToggle);
        GetToggle((int)Toggles.AudioToggle).gameObject.BindEvent(OnClickAudioToggle);
        #endregion

        TogglesInit();
        GetToggle((int)Toggles.DisplayToggle).gameObject.GetComponent<Toggle>().isOn = true;
        OnClickDisplayToggle();


        _AudioPopup.gameObject.SetActive(false);

        return true;
    }

    private void TogglesInit()
    {
        GetObject((int)GameObjects.CheckDisplaymarkObject).SetActive(false);
        GetObject((int)GameObjects.CheckAudiomarkObject).SetActive(false);
    }

    void OnClickDisplayToggle()
    {
        ShowUI(_DisplayPopup.gameObject, GetObject((int)GameObjects.CheckDisplaymarkObject));
        
        //�ϴ� ����
        _AudioPopup.gameObject.SetActive(false);
    }

    void OnClickAudioToggle()
    {
        ShowUI(_AudioPopup.gameObject, GetObject((int)GameObjects.CheckAudiomarkObject));
        _DisplayPopup.gameObject.SetActive(false);
    }
    
    private void ShowUI(GameObject Popup, GameObject checkmark)
    {
        TogglesInit();
        
        Popup.SetActive(true);
        checkmark.SetActive(true);

    }

}
