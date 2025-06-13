using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    
    public BT.State Run(EnemyBase enemy);
}
