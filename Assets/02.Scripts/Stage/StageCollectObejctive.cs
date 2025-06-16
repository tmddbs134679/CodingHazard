using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollectObejctive : MonoBehaviour, IStageObjective
{
    public StageObjectiveType ObjectiveType => StageObjectiveType.Collect;

    [SerializeField] private DroppedItem objectiveItem;

    [SerializeField] private int objectiveCount;

    private int _curCount;

    public void UpdateProgress(out bool isClear)
    {
        _curCount += 1;


        if (_curCount >= objectiveCount)
        {
            isClear = true;
            return;
        }

        isClear = false;
    }
}
