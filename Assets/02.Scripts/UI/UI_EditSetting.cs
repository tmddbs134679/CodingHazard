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

    //임시 연결
    [SerializeField] private UI_AudioPopup _AudioPopup;
    [SerializeField] private UI_DisplayPopup _DisplayPopup;

    private void OnEnable()
    {
        _DisplayPopup.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (_DisplayPopup == null || _AudioPopup == null) return;

        _DisplayPopup.gameObject.SetActive(false);
        _AudioPopup.gameObject.SetActive(false);
    }
    public override bool Init()
    {
        #region Object Bind

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindToggle(typeof(Toggles));
  

        // 토글 클릭 시 행동
        GetToggle((int)Toggles.DisplayToggle).gameObject.BindEvent(OnClickDisplayToggle);
        GetToggle((int)Toggles.AudioToggle).gameObject.BindEvent(OnClickAudioToggle);




        TogglesInit();
        GetToggle((int)Toggles.DisplayToggle).gameObject.GetComponent<Toggle>().isOn = true;
        OnClickDisplayToggle();

        #endregion
        _AudioPopup.gameObject.SetActive(false);

       // Refresh();

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

        //일단 끄기
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
