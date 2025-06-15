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

    //GraphicPopupUI;
    //AudioPopupUI;
    //ControlPopupUI;

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
        GetToggle((int)Toggles.AudioToggle).gameObject.GetComponent<Toggle>().isOn = true;
        OnClickAudioToggle();

        #endregion


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
        ShowUI(/*GraphicPopupUI.gameobject,*/ GetObject((int)GameObjects.CheckDisplaymarkObject));
    }

    void OnClickAudioToggle()
    {
        ShowUI(/*GraphicPopupUI.gameobject,*/ GetObject((int)GameObjects.CheckAudiomarkObject));
    }
    
    private void ShowUI(/*GameObject Popup,*/ GameObject checkmark)
    {
        TogglesInit();

        //Popup.SetActive(true);
        checkmark.SetActive(true);

    }

}
