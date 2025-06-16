using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance => _instance;

    private static StageManager _instance;
    
    public event UnityAction OnCollectedItem;


    [field: SerializeField] public PlayerController PlayerController { get; private set; }


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

    public void CollectItem()
    {
        OnCollectedItem?.Invoke();
    }

    public void ClearStage()
    {
    }
}
