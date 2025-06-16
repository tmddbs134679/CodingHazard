using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
    public event UnityAction OnCollectedItem;

    public void CollectItem()
    {
        OnCollectedItem?.Invoke();
    }

    public void ClearStage()
    {
    }
}
