using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageObjectiveObject : MonoBehaviour
{
    public abstract StageObjectiveType ObjectiveType { get; }

    public Transform MarkerPoint => markerPoint;
    
    [SerializeField] private Transform markerPoint;
    
   
}
