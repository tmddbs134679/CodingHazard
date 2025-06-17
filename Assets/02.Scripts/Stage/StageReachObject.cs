using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageReachObject : StageObjectiveObject
{
    public override StageObjectiveType ObjectiveType { get; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            StageManager.Instance.UpdateObjectives(this);
        }
    }
}
