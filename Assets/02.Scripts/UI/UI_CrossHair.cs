using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CrossHair : UI_Base
{
    enum GameObjects
    {
        CrossHair,
        AimHair,
        DamageCrossHair,
        KillEffectCrossHair
    }
    enum Images
    {
       KillLeft,
       KillRight,
       KillUp,
       KillDown,
    }


    private Vector3 _originalScale;


    private void OnEnable()
    {
        PlayerEvent.OnKillConfirmed += AnimateKillCrosshair;
        PlayerEvent.OnMonsterHit += MonsterHitCrossHair;
        PlayerEvent.OnAttack += AttackAnim;
        PlayerEvent.OnJump += PlayBloomAuto;
        PlayerEvent.OnSprint += HandleSprint;
        PlayerEvent.Aiming += ActiveCrossHair;
    }

  

    private void OnDisable()
    {
        PlayerEvent.OnKillConfirmed -= AnimateKillCrosshair;
        PlayerEvent.OnMonsterHit -= MonsterHitCrossHair;
        PlayerEvent.OnAttack -= AttackAnim;
        PlayerEvent.OnJump -= PlayBloomAuto;
        PlayerEvent.OnSprint -= HandleSprint;
        PlayerEvent.Aiming -= ActiveCrossHair;
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        #endregion


        return true;
    }

    private void Awake()
    {
        Init();
        _originalScale = GetObject((int)GameObjects.AimHair).GetComponent<RectTransform>().localScale;
        GetObject((int)GameObjects.DamageCrossHair).SetActive(false);
        GetObject((int)GameObjects.KillEffectCrossHair).SetActive(false);

        killCrossHairs.Add(GetImage((int)Images.KillLeft));
        killCrossHairs.Add(GetImage((int)Images.KillRight));
        killCrossHairs.Add(GetImage((int)Images.KillUp));
        killCrossHairs.Add(GetImage((int)Images.KillDown));
        foreach (var image in killCrossHairs)
        {
            _originalKillPositions[image] = image.GetComponent<RectTransform>().anchoredPosition;
        }

    }

    public float recoilScaleAmount = 1.3f;

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            AinmateAimCrosshair();
        }
 
    }

    private const float tweenDuration = 0.1f;

    private void AttackAnim()
    {
        if (StageManager.Instance.PlayerController.isAimHold) 
            return;

        ActiveCrossHair(false);
        AnimateRecoilCrosshair(GetObject((int)GameObjects.AimHair));
    }

    private void MonsterHitCrossHair()
    {
        GetObject((int)GameObjects.DamageCrossHair).SetActive(true);
        AnimateRecoilCrosshair(GetObject((int)GameObjects.DamageCrossHair));
        AutoHideAfterDelay(GetObject((int)GameObjects.DamageCrossHair));
    }
    //�Ѿ��� �߻�� �� ����
    private void AnimateRecoilCrosshair(GameObject crossHair)
    {
        crossHair.transform.DOKill();
        crossHair.transform.localScale = _originalScale;

        Sequence seq = DOTween.Sequence();
        Vector3 Objectscale = crossHair.GetComponent<RectTransform>().localScale;
        seq.Append(crossHair.transform.DOScale(Objectscale * recoilScaleAmount, tweenDuration).SetEase(Ease.OutQuad));
        seq.Append(crossHair.transform.DOScale(Objectscale, tweenDuration).SetEase(Ease.InQuad));
  
      
    }


    [SerializeField] private float aimTweenDuration = 0.2f; // Ʈ�� �ð�
    [SerializeField] private float aimShrinkScale = 0.5f;   // �۾��� ����
    private void AinmateAimCrosshair()
    {
        GetObject((int)GameObjects.AimHair).transform.DOKill();


        Sequence seq = DOTween.Sequence();
        seq.Join(GetObject((int)GameObjects.AimHair).transform.DOScale(_originalScale * aimShrinkScale, aimTweenDuration).SetEase(Ease.InOutSine));

        seq.OnComplete(() =>
        {
            ActiveCrossHair(true);
        });
    }
  

    private void ActiveCrossHair(bool isactive)
    {
        GetObject((int)GameObjects.AimHair).gameObject.transform.localScale = _originalScale;
        GetObject((int)GameObjects.CrossHair).SetActive(!isactive);
    }
    private void AutoHideAfterDelay(GameObject crossHair, float delay = 0.2f)
    {
        crossHair.transform.DOKill(); 

        if (!crossHair.activeSelf)
            crossHair.SetActive(true);

        crossHair.transform
            .DOScale(_originalScale, 0) 
            .SetDelay(delay)
            .OnComplete(() => crossHair.SetActive(false));
    }


    private List<Image> killCrossHairs =new List<Image>();
    private Dictionary<Image, Vector2> _originalKillPositions = new Dictionary<Image, Vector2>();
    private Dictionary<Image, Tween> _activeTweens = new Dictionary<Image, Tween>();
    [SerializeField] private float moveDistance = 100f; 
    [SerializeField] private float fadeDuration = 0.5f;
    public void AnimateKillCrosshair()
    {
        GetObject((int)GameObjects.KillEffectCrossHair).SetActive(true);
        foreach (var image in killCrossHairs)
        {
            if (image == null) continue;

            RectTransform rt = image.GetComponent<RectTransform>();
            Vector2 startPos = _originalKillPositions[image];
            Vector2 dir = startPos.normalized;
            Vector2 targetPos = startPos + dir * moveDistance;

        
            if (_activeTweens.ContainsKey(image))
            {
                _activeTweens[image]?.Kill();
                _activeTweens.Remove(image);
            }


            image.gameObject.SetActive(true);
            rt.anchoredPosition = startPos;
            rt.DOKill(); 
            image.DOFade(1f, 0f); 

            Sequence seq = DOTween.Sequence();
            seq.Join(rt.DOAnchorPos(targetPos, fadeDuration).SetEase(Ease.OutCubic));
            seq.Join(image.DOFade(0f, fadeDuration).SetEase(Ease.InQuad));
            seq.AppendCallback(() => {
                rt.anchoredPosition = startPos;

                _activeTweens.Remove(image);
            });

            _activeTweens[image] = seq;
        }
    }



    public RectTransform aimHair;
    public float bloomScale = 1.5f;
    public float bloomTime = 0.1f;
    public float shrinkTime = 0.2f;


    void HandleSprint(bool isSprinting)
    {
        if (isSprinting)
            PlayBloom();
        else
            ShrinkBack();
    }
    public void PlayBloom()
    {
        DOTween.Kill(GetObject((int)GameObjects.AimHair)); // 중복 방지

        GetObject((int)GameObjects.AimHair).GetComponent<RectTransform>().DOScale(_originalScale * bloomScale, bloomTime).SetEase(Ease.OutQuad);
    }

    public void ShrinkBack()
    {
        GetObject((int)GameObjects.AimHair).GetComponent<RectTransform>().DOScale(_originalScale, shrinkTime).SetEase(Ease.OutQuad);
    }

    // 자동으로 수축되는 버전
    public void PlayBloomAuto()
    {
        PlayBloom();
        DOVirtual.DelayedCall(bloomTime + 0.05f, () => ShrinkBack());
    }

}
