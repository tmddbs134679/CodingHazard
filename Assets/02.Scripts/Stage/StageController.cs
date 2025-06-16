using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
    public event UnityAction OnCollectedItem;
    
    [SerializeField] private int requiredCollectCount;
    [SerializeField] private GameObject collectItem;

    private int _curCollectCount;

    public void CollectItem(int count)
    {
        _curCollectCount += count;
        
        OnCollectedItem?.Invoke();

        if (_curCollectCount >= requiredCollectCount)
        {
            ClearStage();
        }
    }

    public void ClearStage()
    {
    }
}
