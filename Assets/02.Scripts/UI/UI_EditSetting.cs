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
        CheckGraphicmarkObject,
        CheckAudiomarkObject,
        CheckControlmarkObject,

    }
    enum Buttons
    {

    }
    enum Texts
    {

    }
    enum Toggles
    {
        GraphicToggle,
        AudioToggle,
        ControlToggle,
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
        GetToggle((int)Toggles.GraphicToggle).gameObject.BindEvent(OnClickGraphicToggle);
        GetToggle((int)Toggles.AudioToggle).gameObject.BindEvent(OnClickAudioToggle);
        GetToggle((int)Toggles.ControlToggle).gameObject.BindEvent(OnClickControlToggle);



        TogglesInit();
        GetToggle((int)Toggles.AudioToggle).gameObject.GetComponent<Toggle>().isOn = true;
        OnClickAudioToggle();

        #endregion


       // Refresh();

        return true;
    }

    private void TogglesInit()
    {
        GetObject((int)GameObjects.CheckGraphicmarkObject).SetActive(false);
        GetObject((int)GameObjects.CheckAudiomarkObject).SetActive(false);
        GetObject((int)GameObjects.CheckControlmarkObject).SetActive(false);
    }

    void OnClickGraphicToggle()
    {
        ShowUI(/*GraphicPopupUI.gameobject,*/ GetObject((int)GameObjects.CheckGraphicmarkObject));
    }

    void OnClickAudioToggle()
    {
        ShowUI(/*GraphicPopupUI.gameobject,*/ GetObject((int)GameObjects.CheckAudiomarkObject));
    }
    
    void OnClickControlToggle()
    {
        ShowUI(/*GraphicPopupUI.gameobject,*/ GetObject((int)GameObjects.CheckControlmarkObject));
    }

    private void ShowUI(/*GameObject Popup,*/ GameObject checkmark)
    {
        TogglesInit();

        //Popup.SetActive(true);
        checkmark.SetActive(true);

    }

}
