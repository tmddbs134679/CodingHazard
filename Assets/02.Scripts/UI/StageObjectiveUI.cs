using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StageObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI progressTMPPrefab;
    
    [SerializeField] private Transform progressTMPRoot;
    
    [SerializeField] private RectTransform hidePoint;
    [SerializeField] private RectTransform showPoint;
    [SerializeField] private RectTransform mapMarker;
        

    private TextMeshProUGUI _curTMP;
    
    private StageManager _stageManager;

    private string _distanceText;
    
    private Camera _mainCamera;

    
    private void Start()    
    {
        _mainCamera = Camera.main;
        
        _stageManager = StageManager.Instance;

        _stageManager.OnChangedObjective += SetTitle;
        _stageManager.OnChangedObjective += ShowProgress;
        
        SetTitle(_stageManager.CurObjective);
        ShowProgress(_stageManager.CurObjective);
    }

    private void OnDestroy()
    {
        _stageManager.OnChangedObjective -= SetTitle;
        _stageManager.OnChangedObjective -= ShowProgress;
    }

    private void Update()
    {
        var targetObjective = _stageManager.CurObjective;
        
        if (targetObjective != null)
        {
            var targetObject = targetObjective.GetTargetObjectiveObject();

            if (targetObject != null)
            {
                Vector3 screenPos = _mainCamera.WorldToScreenPoint(targetObject.MarkerPoint.position);
                
                if (screenPos.z >= 0)
                {
                    mapMarker.position = screenPos;
                }

                Vector3 diff = _stageManager.PlayerController.transform.position - targetObject.MarkerPoint.position;

                _curTMP.text = $"•  {targetObjective.Description} {targetObjective.GetProgressText()}  ( {(int)diff.magnitude}M )";
            }
        }
    }


    private void SetTitle(StageObjective objective)
    {
        titleText.text = GetTitleText(objective.ObjectiveType);
    }

    private string GetTitleText(StageObjectiveType type)
    {
        return type switch
        {
            StageObjectiveType.Collect => "수집",
            StageObjectiveType.Kill => "처치",
            StageObjectiveType.Reach => "도달",
            _=>""
        };
    }
    
    private void ShowProgress(StageObjective objective)
    {
        Color targetColor = progressTMPPrefab.color;
        
        if (_curTMP != null)
        {
            targetColor.a = 0;

            var preTMP = _curTMP;
            
            preTMP.DOColor(targetColor, 0.5f).OnComplete(() => Destroy(preTMP.gameObject));
        }

        targetColor.a = 1;
        
        _curTMP = Instantiate(progressTMPPrefab, progressTMPRoot);
        
        _curTMP.transform.position = hidePoint.transform.position;

        _curTMP.color = targetColor;
        
        _curTMP.transform.DOMove(showPoint.position, 1f);
    }
}
