using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI progressText;


    private StageManager _stageManager;
    private void Start()
    {
        _stageManager = StageManager.Instance;

        _stageManager.OnChangedObjective += SetTitle;
        _stageManager.OnChangedObjective += SetProgress;

        _stageManager.OnObjectiveUpdatedProgress += SetProgress;

        if (_stageManager.Objectives.Count > 0)
        {
            var firstObjective = _stageManager.Objectives[0];

            SetTitle(firstObjective);
            SetProgress(firstObjective);
        }
     
    }

    private void OnDestroy()
    {
        _stageManager.OnChangedObjective -= SetTitle;
        _stageManager.OnChangedObjective -= SetProgress;

        _stageManager.OnObjectiveUpdatedProgress -= SetProgress;
    }


    private void SetTitle(StageObjective objective)
    {
        titleText.text = GetTitleText(objective.ObjectiveType);
    }
    
    private void SetProgress(StageObjective objective)
    {
        progressText.text = $"• {objective.Description} {objective.GetProgressText()}";
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
}
