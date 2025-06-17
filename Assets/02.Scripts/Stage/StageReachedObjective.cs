using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageReachedObjective : StageObjective
{
    public override StageObjectiveType ObjectiveType => StageObjectiveType.Reach;
    
    
    public StageReachObject Objective => objective;

    [SerializeField] private StageReachObject objective;

    private PlayerController _player;
    
    public override void Enter()
    {
        _player = StageManager.Instance.PlayerController;
    }

    public override string GetProgressText()
    {
        return $"({(int)Vector3.Distance(_player.transform.position, objective.transform.position)}m)";
    }

    public override bool TryUpdateProgress<T>(T targret, out bool isComplete)
    {
        isComplete = true;
        return true;
    }
}


