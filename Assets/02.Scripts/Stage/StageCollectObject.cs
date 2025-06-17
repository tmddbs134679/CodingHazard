using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollectObject : StageObjectiveObject
{
    public override StageObjectiveType ObjectiveType => StageObjectiveType.Collect;

    public void Collect()
    {
        ToggleOutline(false);
        
        StageManager.Instance.UpdateObjectives(this);
    }
}
