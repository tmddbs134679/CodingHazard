using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStageObjective
{
    public StageObjectiveType ObjectiveType { get; }

    public void UpdateProgress(out bool isClear);
}
