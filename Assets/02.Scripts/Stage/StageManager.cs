using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance => _instance;

    private static StageManager _instance;

    public event UnityAction<StageObjective> OnChangedObjective;
    public event Action OnClearStage;
    public Action OnFailStage;

    [field: SerializeField] public PlayerController PlayerController { get; private set; }

    public StageObjective CurObjective { get; private set; }
    

    [SerializeField] private Transform objectivesRoot;
    
    [SerializeField] private FadeScreen fadeScreen;

    
    private List<StageObjective> _objectives = new();


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _objectives = objectivesRoot.GetComponentsInChildren<StageObjective>().ToList();
        
        
        for (int i = 0; i < _objectives.Count; i++)
        {
            _objectives[i].Init(this);
            
            if (i == 0)
            {
                CurObjective = _objectives[i];
                
                CurObjective.Enter();
            }
        }
    }
    


    public void UpdateObjectives<T>(T target) where T : StageObjectiveObject
    {
        if (_objectives.Count > 0)
        {
            var objective = _objectives[0];
            
            objective.UpdateProgress(target, out bool isClear);

            if (isClear)
            {
                ChangeProgress();
            }
        }
    }

    public void ClearStage()
    {
        PlayerController.BlockInput();

        OnClearStage?.Invoke();
    }
    
    private void ChangeProgress()
    {
        _objectives.RemoveAt(0);

        if (_objectives.Count == 0)
        {
            ClearStage();
        }
        else
        {
            CurObjective = _objectives[0];
                        
            OnChangedObjective?.Invoke(CurObjective);
                        
            CurObjective.Enter();
        }
    }
}
