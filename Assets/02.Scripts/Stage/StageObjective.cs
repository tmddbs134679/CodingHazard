using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class StageObjective : MonoBehaviour
{
    public abstract StageObjectiveType ObjectiveType { get; }


    [field: SerializeField] public string Description { get; private set; }
    
    [Space(10f)]
    [SerializeField] protected UnityEvent onCompleteEvent;

    
    public abstract string GetProgressText();
    public abstract bool TryUpdateProgress<T>(T targret, out bool isComplete)  where T : MonoBehaviour;
  
}
