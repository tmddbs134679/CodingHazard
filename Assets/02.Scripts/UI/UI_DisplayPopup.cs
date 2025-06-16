using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DisplayPopup : UI_Base
{

    #region Enum

    enum GameObjects
    {
        SensitivitySlider,
        FovSlider,
        DOFActiveObject,
        DOFInActiveObject,
        AAActiveObject,
        AAInActiveObject,
        MotionBlurActiveObject,
        MotionBlurInActiveObject

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

        foreach (var kvp in Sliders)
        {
            GameObjects type = kvp.Key;
            kvp.Value.onValueChanged.AddListener((val) => UpdateSliderText(type, val));
            UpdateSliderText(type, kvp.Value.value);
        }
        return true;
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
    }

}
