using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;


public class UI_GameScene : UI_Base
{
    #region Enum
  
    enum Buttons
    {
  
    }
    enum Texts
    {
     
    }
    enum Images
    {
       PrimarySlot,
       MeleeSlot,
       OpenEye,
       CloseEye,
       BloodScreen,
    }
    #endregion

    private List<Image> weaponIcons = new List<Image>();    
    private Color whiteColor = Color.white;
    private Color choiceColor = new Color32(0x9C, 0xCF, 0x42, 0xFF);
    private const float fadeDuration = 5f;
    private float testhp = 100f;
    private float testStaminaF = 100f;
    [SerializeField] private UI_HPBar testHP;
    [SerializeField] private UI_StaminaBar testStamina;
    [SerializeField] private UI_EditSetting testSetting;


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindImage(typeof(Images));
        #endregion

        GetImage((int)Images.MeleeSlot).gameObject.SetActive(false);
        GetImage((int)Images.PrimarySlot).gameObject.SetActive(false);
        GetImage((int)Images.OpenEye).gameObject.SetActive(false);

        weaponIcons.Add(GetImage((int)Images.PrimarySlot));
        weaponIcons.Add(GetImage((int)Images.MeleeSlot));

        return true;
    }

    private void Awake()
    {
        Init();
       
    }

    private void OnEnable()
    {
        PlayerEvent.Swap += UpdateQuickSlot;
        PlayerEvent.Swap += UpdateQuickSlot;
    }

    private void Update()
    {
        //Test

        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    UpdateQuickSlot(0);
        //}
        //else if(Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    UpdateQuickSlot(1);
        //}

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            DetectionEyeVisible(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DetectionEyeVisible(false);
        }

        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    testHP.GetComponent<UI_HPBar>().SetHpRatio(StageManager.Instance.PlayerController.GetComponent<PlayerCondition>().hp.curValue-= 20f);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    testStamina.GetComponent<UI_StaminaBar>().SetStaminaRatio(ref testStaminaF);
        //}

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            UpdateBloodScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            testSetting.gameObject.SetActive(!testSetting.gameObject.activeSelf);
        }

    }


    private Sequence currentSequence; 
    private void UpdateQuickSlot(int selectedWeaponIndex)
    {
        selectedWeaponIndex -= 1;
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill(); 
        }


        foreach (var icon in weaponIcons)
        {
            DOTween.Kill(icon); 
        }

     
        foreach (var icon in weaponIcons)
        {
            icon.gameObject.SetActive(true);
            icon.color = whiteColor;
            icon.DOFade(1f, 0f);
        }

        Image selectedIcon = weaponIcons[selectedWeaponIndex];
        selectedIcon.color = choiceColor;

        currentSequence = DOTween.Sequence();
        foreach (var icon in weaponIcons)
        {
            currentSequence.Join(icon.DOFade(0f, 5f));
        }

        currentSequence.OnComplete(() =>
        {
            foreach (var icon in weaponIcons)
            {
                icon.color = whiteColor;
                icon.DOFade(1f, 0f);
                icon.gameObject.SetActive(false);
            }
        });
    }


    private Tween pulseTween;
    private void DetectionEyeVisible(bool isVisible)
    {
        GetImage((int)Images.OpenEye).gameObject.SetActive(isVisible);
        GetImage((int)Images.CloseEye).gameObject.SetActive(!isVisible);

        if (isVisible)
        {
            
            if (pulseTween == null || !pulseTween.IsActive())
            {
                pulseTween = GetImage((int)Images.OpenEye).transform.DOScale(1.2f, 0.2f)
                    .SetLoops(-1, LoopType.Yoyo) //루프
                    .SetEase(Ease.InOutSine);   //속도
            }
        }
        else
        {
          
            if (pulseTween != null) pulseTween.Kill();
            pulseTween = null;
            GetImage((int)Images.OpenEye).transform.localScale = Vector3.one;
        }
    }


    private Tween _bloodTween;
    private void UpdateBloodScreen()
    {
        _bloodTween?.Kill();

        Color color = GetImage((int)Images.BloodScreen).color;
        color.a = 0f;
        GetImage((int)Images.BloodScreen).color = color;
        _bloodTween = GetImage((int)Images.BloodScreen).DOFade(0.3f, 0.2f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }



}
