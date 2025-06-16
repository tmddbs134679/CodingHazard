using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static ect;

public class UI_DisplayPopup : UI_Base
{

    #region Enum

    enum GameObjects
    {
        MouseSensitivity,
        FOV,
        DOF,
        AA,
        MotionBlur,
        Mousebg,
        FOVbg,
        DOFbg,
        AAbg,
        MotionBlurbg,
        SensitivitySlider,
        FovSlider,
        DOFActiveObject,
        DOFInActiveObject,
        AAActiveObject,
        AAInActiveObject,
        MotionBlurActiveObject,
        MotionBlurInActiveObject,

    }
    enum Toggles
    {
        DOFToggle,
        AAToggle,
        MotionBlurToggle
    }

    enum Texts
    {
        SensitiveValueText,
        FovValueText
    }

    #endregion
    Dictionary<GameObjects, Slider> Sliders = new Dictionary<GameObjects, Slider>();
    [SerializeField] private Dictionary<GameObjects, TMP_Text> SliderTexts = new Dictionary<GameObjects, TMP_Text>();
    public override bool Init()
    {
        #region Object Bind

        BindObject(typeof(GameObjects));
        BindToggle(typeof(Toggles));
        BindText(typeof(Texts));

        TogglesInit();
        BackGroundInit();

        #endregion
        GetToggle((int)Toggles.DOFToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.DOFToggle),
                GetObject((int)GameObjects.DOFActiveObject),
                GetObject((int)GameObjects.DOFInActiveObject)
            );
        });
        GetToggle((int)Toggles.AAToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.AAToggle),
                GetObject((int)GameObjects.AAActiveObject),
                GetObject((int)GameObjects.AAInActiveObject)
            );
        });
        GetToggle((int)Toggles.MotionBlurToggle).gameObject.BindEvent(() =>
        {
            OnClickToggle(
                GetToggle((int)Toggles.MotionBlurToggle),
                GetObject((int)GameObjects.MotionBlurActiveObject),
                GetObject((int)GameObjects.MotionBlurInActiveObject)
            );
        });

        Sliders.Add(GameObjects.SensitivitySlider, GetObject((int)GameObjects.SensitivitySlider).GetComponent<Slider>());
        Sliders.Add(GameObjects.FovSlider, GetObject((int)GameObjects.FovSlider).GetComponent<Slider>());
        SliderTexts.Add(GameObjects.SensitivitySlider, GetText((int)Texts.SensitiveValueText));
        SliderTexts.Add(GameObjects.FovSlider, GetText((int)Texts.FovValueText));

        HoverInit();


        foreach (var kvp in Sliders)
        {
            GameObjects type = kvp.Key;
            kvp.Value.onValueChanged.AddListener((val) => UpdateSliderText(type, val));
          

          
            if (type == GameObjects.SensitivitySlider)
            {
                kvp.Value.value = GameManager.Instance.GraphicsSettingManager.MouseSensitivity;

                kvp.Value.onValueChanged.AddListener(GameManager.Instance.GraphicsSettingManager.SetMouseSensitivity);
            }
              

            else if (type == GameObjects.FovSlider)
            {
                kvp.Value.value = GameManager.Instance.GraphicsSettingManager.FOV;
                kvp.Value.onValueChanged.AddListener(GameManager.Instance.GraphicsSettingManager.SetFov);
            }

            UpdateSliderText(type, kvp.Value.value);
        }
        return true;
    }

    private void BackGroundInit()
    {
        GetObject((int)GameObjects.Mousebg).SetActive(false);
        GetObject((int)GameObjects.FOVbg).SetActive(false);
        GetObject((int)GameObjects.DOFbg).SetActive(false);
        GetObject((int)GameObjects.AAbg).SetActive(false);
        GetObject((int)GameObjects.MotionBlurbg).SetActive(false);
    }

    private void HoverInit()
    {
        GetObject((int)GameObjects.MouseSensitivity).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.MouseSensitivity), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.MouseSensitivity).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.MouseSensitivity), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.FOV).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.FOV), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.FOV).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.FOV), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.DOF).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.DOF), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.DOF).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.DOF), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.AA).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.AA), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.AA).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.AA), false), null, EUIEvent.PointerExit);
        GetObject((int)GameObjects.MotionBlur).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.MotionBlur), true), null, EUIEvent.PointerEnter);
        GetObject((int)GameObjects.MotionBlur).BindEvent(() => OnHoverSetting(GetObject((int)GameObjects.MotionBlur), false), null, EUIEvent.PointerExit);

    }

 

    void UpdateSliderText(GameObjects type, float value)
    {
        SliderTexts[type].text = $"{Mathf.RoundToInt(value * 100)}";
    }

    private void TogglesInit()
    {
        GetObject((int)GameObjects.DOFInActiveObject).SetActive(false);
        GetObject((int)GameObjects.AAInActiveObject).SetActive(false);
        GetObject((int)GameObjects.MotionBlurInActiveObject).SetActive(false);
    }


    private void OnClickToggle(Toggle tgl, GameObject activeObj, GameObject inactiveObj)
    {
        bool isOn = tgl.isOn;
        activeObj.SetActive(isOn);
        inactiveObj.SetActive(!isOn);

        if (tgl == GetToggle((int)Toggles.DOFToggle))
            GameManager.Instance.GraphicsSettingManager.ToggleVolumeComponent<DepthOfField>(isOn);

        else if (tgl == GetToggle((int)Toggles.AAToggle))
        {
            AntialiasingMode mode = (AntialiasingMode)(isOn ? 1 : 0);
            GameManager.Instance.GraphicsSettingManager.SetAntiAliasingMode(mode);
        }
           

        else if (tgl == GetToggle((int)Toggles.MotionBlurToggle))
            GameManager.Instance.GraphicsSettingManager.ToggleVolumeComponent<MotionBlur>(isOn);
    }

 
}
