using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;


public class UI_GameScene : UI_Base
{
    #region Enum
    enum GameObjects
    {
    
    }
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
    }
    #endregion

    private List<Image> weaponIcons = new List<Image>();    
    private Color whiteColor = Color.white;
    private Color choiceColor = new Color32(0x9C, 0xCF, 0x42, 0xFF);
    private const float fadeDuration = 5f;

    [SerializeField] private UI_HPBar testHP;
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

    private void Update()
    {
        //Test

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateQuickSlot(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpdateQuickSlot(1);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            DetectionEyeVisible(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            testHP.GetComponent<UI_HPBar>().SetHpRatio(50f);
        }
    }


    private Sequence currentSequence; 
    private void UpdateQuickSlot(int selectedWeaponIndex)
    {

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



    private void DetectionEyeVisible(bool isVisible)
    {
        GetImage((int)Images.OpenEye).gameObject.SetActive(isVisible);
        GetImage((int)Images.CloseEye).gameObject.SetActive(!isVisible);
    }




}
