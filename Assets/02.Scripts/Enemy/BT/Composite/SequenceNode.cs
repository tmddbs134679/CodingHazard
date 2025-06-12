using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : CompositeNode
{
    public override BT.State Run(EnemyBase enemy)
    {
        foreach (INode child in children) { 
            var result = child.Run(enemy);
            if (result != BT.State.Sucess)
            {
                return result;
            }
        }
        return BT.State.Sucess;
    }
}
