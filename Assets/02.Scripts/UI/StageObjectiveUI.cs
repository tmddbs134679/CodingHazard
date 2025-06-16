using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI progress;


    private StageManager _stageManager;
    private void Start()
    {
        _stageManager = StageManager.Instance;

        _stageManager.OnChangedObjective += SetDescription;
        _stageManager.OnChangedObjective += SetProgress;

        _stageManager.OnObjectiveUpdatedProgress += SetProgress;

        if (_stageManager.Objectives.Count > 0)
        {
            var firstObjective = _stageManager.Objectives[0];

            SetDescription(firstObjective);
            SetProgress(firstObjective);
        }
     
    }

    private void OnDestroy()
    {
        _stageManager.OnChangedObjective -= SetDescription;
        _stageManager.OnChangedObjective -= SetProgress;

        _stageManager.OnObjectiveUpdatedProgress -= SetProgress;
    }


    private void SetDescription(StageObjective objective)
    {
        description.text = objective.Description;
    }
    
    private void SetProgress(StageObjective objective)
    {
        progress.text = objective.GetProgressText();
    }
}
