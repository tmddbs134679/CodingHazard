using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionNode : INode
{
    
    public abstract BT.State Run(EnemyBase enemy);


}
