using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ect;

public class UI_AudioPopup : UI_Base
{
    #region Enum
    enum GameObjects
    {
        MasterObject,
        BgmObject,
        SFXObject,
        VoiceObject,
        MasterVloume,
        BGMVloume,
        SFXVolume,
        VoiceVolume,
        Master,
        BGM,
        SFX,
        Voice,
        MasterActiveObject,
        MasterInActiveObject,
        BGMActiveObject,
        BGMInActiveObject,
        SFXActiveObject,
        SFXInActiveObject,
        VoiceActiveObject,
        VoiceInActiveObject,
        Masterbg,
        MasterVloumebg,
        BGMbg,
        BGMVloumebg,
        SFXbg,
        SFXVolumebg,
        Voicebg,
        VoiceVolumebg,
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


        HoverInit();
        #endregion

        #region Toggle Bind
        GetToggle((int)Toggles.MasterToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.MasterToggle),
                GetObject((int)GameObjects.MasterActiveObject),
                GetObject((int)GameObjects.MasterInActiveObject),
                GetObject((int)GameObjects.MasterVloume),
                AudioType.Master
            );
        });

        GetToggle((int)Toggles.BgmToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.BgmToggle),
                GetObject((int)GameObjects.BGMActiveObject),
                GetObject((int)GameObjects.BGMInActiveObject),
                GetObject((int)GameObjects.BGMVloume),
                AudioType.BGM
            );
        });
        GetToggle((int)Toggles.SFXToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.SFXToggle),
                GetObject((int)GameObjects.SFXActiveObject),
                GetObject((int)GameObjects.SFXInActiveObject),
                GetObject((int)GameObjects.SFXVolume),
                AudioType.SFX
            );
        });
        GetToggle((int)Toggles.VoiceToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.VoiceToggle),
                GetObject((int)GameObjects.VoiceActiveObject),
                GetObject((int)GameObjects.VoiceInActiveObject),
                GetObject((int)GameObjects.VoiceVolume),
                AudioType.Voice
            );
        });

        #endregion

        #region Slider or Text Add
        volumeSliders.Add(GameObjects.Master, GetObject((int)GameObjects.Master).GetComponent<Slider>());
        volumeTexts.Add(GameObjects.Master, GetText((int)Texts.MasterValueText));
        volumeSliders.Add(GameObjects.BGM, GetObject((int)GameObjects.BGM).GetComponent<Slider>());
        volumeTexts.Add(GameObjects.BGM, GetText((int)Texts.BGMValueText));
        volumeSliders.Add(GameObjects.SFX, GetObject((int)GameObjects.SFX).GetComponent<Slider>());
        volumeTexts.Add(GameObjects.SFX, GetText((int)Texts.SFXValueText));
        volumeSliders.Add(GameObjects.Voice, GetObject((int)GameObjects.Voice).GetComponent<Slider>());
        volumeTexts.Add(GameObjects.Voice, GetText((int)Texts.VoiceValueText));

        #endregion


        TogglesInit();
        BackgroundInit();

        foreach (var kvp in volumeSliders)
        {
            GameObjects uiEnum = kvp.Key;
            Slider slider = kvp.Value;

            // GameObjects → AudioType 변환 (enum 이름이 동일하다는 전제)
            if (!System.Enum.TryParse(uiEnum.ToString(), out AudioType audioType))
                continue;

            // 슬라이더 변경 시 텍스트 + 사운드 반영
            slider.onValueChanged.AddListener((val) =>
            {
                UpdateVolumeText(uiEnum, val);  
               AudioManager.Instance.SetVolume(audioType, val); // 사운드 반영
            });

            // 초기화 시 반영
            UpdateVolumeText(uiEnum, slider.value);
            AudioManager.Instance.SetVolume(audioType, slider.value);
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

    private void OnClickToggle(Toggle tgl, GameObject activeObj, GameObject inactiveObj, GameObject volumeObj, AudioType type)
    {
        bool isOn = tgl.isOn;
        activeObj.SetActive(isOn);
        inactiveObj.SetActive(!isOn);

        volumeObj.SetActive(isOn);

        // Todo : 비활성화 상태이면 볼륨 0만들기
        if (!isOn)
        {
            // 비활성화 시 볼륨 0
            AudioManager.Instance.SetVolume(type, 0f);
        }
        else
        {
            foreach (var kvp in volumeSliders)
            {
                GameObjects uiEnum = kvp.Key;
                Slider slider = kvp.Value;
                if (!System.Enum.TryParse(uiEnum.ToString(), out AudioType audioType))
                    continue;
                // 초기화 시 반영
                UpdateVolumeText(uiEnum, slider.value);
                AudioManager.Instance.SetVolume(audioType, slider.value);
            }
        }
    }

    void UpdateVolumeText(GameObjects type, float value)
    {
        volumeTexts[type].text = $"{Mathf.RoundToInt(value * 100)}";
    }
    private void BackgroundInit()
    {
        GetObject((int)GameObjects.MasterVloumebg).SetActive(false);
        GetObject((int)GameObjects.Masterbg).SetActive(false);
        GetObject((int)GameObjects.BGMVloumebg).SetActive(false);
        GetObject((int)GameObjects.BGMbg).SetActive(false);
        GetObject((int)GameObjects.SFXVolumebg).SetActive(false);
        GetObject((int)GameObjects.SFXbg).SetActive(false);
        GetObject((int)GameObjects.VoiceVolumebg).SetActive(false);
        GetObject((int)GameObjects.Voicebg).SetActive(false);
    }

    private void HoverInit()
    {
        GetObject((int)GameObjects.MasterObject).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.MasterObject), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.MasterObject).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.MasterObject), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.MasterVloume).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.MasterVloume), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.MasterVloume).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.MasterVloume), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.BgmObject).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.BgmObject), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.BgmObject).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.BgmObject), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.BGMVloume).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.BGMVloume), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.BGMVloume).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.BGMVloume), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.SFXObject).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.SFXObject), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.SFXObject).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.SFXObject), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.SFXVolume).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.SFXVolume), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.SFXVolume).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.SFXVolume), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.VoiceObject).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.VoiceObject), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.VoiceObject).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.VoiceObject), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.VoiceVolume).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.VoiceVolume), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.VoiceVolume).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.VoiceVolume), false), null, EUIEvent.PointerExit);
    }
}
