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

    private StageManager _stageManager;

    private TextMeshProUGUI _curTMP;

    private Dictionary<StageObjective, TextMeshProUGUI> progressTMPDict = new();

    
    private void Start()
    {
        _stageManager = StageManager.Instance;

        _stageManager.OnChangedObjective += SetTitle;
        _stageManager.OnChangedObjective += ShowProgress;

        _stageManager.OnObjectiveUpdatedProgress += SetProgress;

        for (int i = 0; i < _stageManager.Objectives.Count; i++)
        {
            var targetObjective = _stageManager.Objectives[i];
            
            var text = Instantiate(progressTMPPrefab, progressTMPRoot);

            text.transform.position = hidePoint.transform.position;
            
            text.gameObject.SetActive(false);

            progressTMPDict[targetObjective] = text;
            
            SetProgress(targetObjective);

            if (i == 0)
            {
                SetTitle(targetObjective);
                ShowProgress(targetObjective);
            }
        }
    }

    private void OnDestroy()
    {
        _stageManager.OnChangedObjective -= SetTitle;
        _stageManager.OnChangedObjective -= ShowProgress;

        _stageManager.OnObjectiveUpdatedProgress -= SetProgress;
    }


    private void SetTitle(StageObjective objective)
    {
        titleText.text = GetTitleText(objective.ObjectiveType);
    }
    
    private void SetProgress(StageObjective objective)
    {
        progressTMPDict[objective].text = $"•  {objective.Description} {objective.GetProgressText()}";
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

        _curTMP = progressTMPDict[objective];

        _curTMP.gameObject.SetActive(true);

        _curTMP.color = targetColor;
        
        _curTMP.transform.DOMove(showPoint.position, 1f);
    }
}
