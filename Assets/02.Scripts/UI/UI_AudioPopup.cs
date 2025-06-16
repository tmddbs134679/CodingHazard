using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AudioPopup : UI_Base
{
    #region Enum
    enum GameObjects
    {
        MasterVloume,
        BGMVloume,
        SFXVolume,
        VoiceVolume,
        MasterSlider,
        BGMSlider,
        SFXSlider,
        VoiceSlider,
        MasterActiveObject,
        MasterInActiveObject,
        BGMActiveObject,
        BGMInActiveObject,
        SFXActiveObject,
        SFXInActiveObject,
        VoiceActiveObject,
        VoiceInActiveObject,
    }
    enum Buttons
    {

    }
    enum Texts
    {
        MasterValueText,
        BGMValueText,
        SFXValueText,
        VoiceValueText,
    }

    enum Toggles
    {
        MasterToggle,
        BgmToggle,
        SFXToggle,
        VoiceToggle,
    }

    #endregion

    Dictionary<GameObjects, Slider> volumeSliders = new Dictionary<GameObjects, Slider>();
    [SerializeField] private Dictionary<GameObjects, TMP_Text> volumeTexts = new Dictionary<GameObjects, TMP_Text>();
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindToggle(typeof(Toggles));

     


        #endregion

        GetToggle((int)Toggles.MasterToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.MasterToggle),
                GetObject((int)GameObjects.MasterActiveObject),
                GetObject((int)GameObjects.MasterInActiveObject),
                GetObject((int)GameObjects.MasterVloume)
            );
        });

        GetToggle((int)Toggles.BgmToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.BgmToggle),
                GetObject((int)GameObjects.BGMActiveObject),
                GetObject((int)GameObjects.BGMInActiveObject),
                GetObject((int)GameObjects.BGMVloume)
            );
        });
        GetToggle((int)Toggles.SFXToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.SFXToggle),
                GetObject((int)GameObjects.SFXActiveObject),
                GetObject((int)GameObjects.SFXInActiveObject),
                GetObject((int)GameObjects.SFXVolume)
            );
        });
        GetToggle((int)Toggles.VoiceToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.VoiceToggle),
                GetObject((int)GameObjects.VoiceActiveObject),
                GetObject((int)GameObjects.VoiceInActiveObject),
                GetObject((int)GameObjects.VoiceVolume)
            );
        });

        volumeSliders.Add(GameObjects.MasterSlider, GetObject((int)GameObjects.MasterSlider).GetComponent<Slider>());
        volumeTexts.Add(GameObjects.MasterSlider, GetText((int)Texts.MasterValueText));
        volumeSliders.Add(GameObjects.BGMSlider, GetObject((int)GameObjects.BGMSlider).GetComponent<Slider>());
        volumeTexts.Add(GameObjects.BGMSlider, GetText((int)Texts.BGMValueText));
        volumeSliders.Add(GameObjects.SFXSlider, GetObject((int)GameObjects.SFXSlider).GetComponent<Slider>());
        volumeTexts.Add(GameObjects.SFXSlider, GetText((int)Texts.SFXValueText));
        volumeSliders.Add(GameObjects.VoiceSlider, GetObject((int)GameObjects.VoiceSlider).GetComponent<Slider>());
        volumeTexts.Add(GameObjects.VoiceSlider, GetText((int)Texts.VoiceValueText));
        TogglesInit();
        // Refresh();

        foreach (var kvp in volumeSliders)
        {
            GameObjects type = kvp.Key;
            kvp.Value.onValueChanged.AddListener((val) => UpdateVolumeText(type, val));

  
            UpdateVolumeText(type, kvp.Value.value);
        }
        return true;
    }

    private void TogglesInit()
    {
        GetToggle((int)Toggles.MasterToggle).isOn = true;
        GetToggle((int)Toggles.BgmToggle).isOn = true;
        GetToggle((int)Toggles.SFXToggle).isOn = true;
        GetToggle((int)Toggles.VoiceToggle).isOn = true;
        GetObject((int)GameObjects.MasterInActiveObject).SetActive(false);
        GetObject((int)GameObjects.BGMInActiveObject).SetActive(false);
        GetObject((int)GameObjects.SFXInActiveObject).SetActive(false);
        GetObject((int)GameObjects.VoiceInActiveObject).SetActive(false);


    }

    private void OnClickToggle(Toggle tgl, GameObject activeObj, GameObject inactiveObj, GameObject volumeObj)
    {
        bool isOn = tgl.isOn;
        activeObj.SetActive(isOn);
        inactiveObj.SetActive(!isOn);

        volumeObj.SetActive(isOn);

        // Todo : 비활성화 상태이면 볼륨 0만들기
    }

    void UpdateVolumeText(GameObjects type, float value)
    {
        volumeTexts[type].text = $"{Mathf.RoundToInt(value * 100)}";
    }
}
