using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance => _instance;

    private static StageManager _instance;

    public event UnityAction<StageObjective> OnObjectiveUpdatedProgress;
    public event UnityAction<StageObjective> OnChangedObjective;
    

    [field: SerializeField] public PlayerController PlayerController { get; private set; }

    public List<StageObjective> Objectives => objectives;

    [SerializeField] private List<StageObjective> objectives;
    
    [SerializeField] private FadeScreen fadeScreen;
    
    [SerializeField] private GameObject tempResultUI;
    [SerializeField] private Button tempResultButton;
    

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
    }

    public void UpdateObjectives<T>(T target) where T : MonoBehaviour
    {
        if (objectives.Count > 0)
        {
            var objective = objectives[0];
            
            if (objective.TryUpdateProgress(target, out bool isComplete))
            {
                OnObjectiveUpdatedProgress?.Invoke(objective);

                
                if (isComplete)
                {
                    if (objectives.Count == 1)
                    {
                        ClearStage();
                    }
                    else
                    {
                        var nextObjective = objectives[1];
                        
                        OnChangedObjective?.Invoke(nextObjective);
                    }
                    
                    objectives.RemoveAt(0);
                }
            }
        }
    }

    public void ClearStage()
    {
        fadeScreen.FadeOut(
            () =>
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                
                fadeScreen.gameObject.SetActive(false);
                tempResultUI.gameObject.SetActive(true);
            });
        
        tempResultButton.onClick.AddListener(() =>
        {
            SceneLoadManager.Instance.LoadScene("LobbyScene");

            Time.timeScale = 1;
        });
        
        Time.timeScale = 0;
    }
}
