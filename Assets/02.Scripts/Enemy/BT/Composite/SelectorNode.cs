using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    public override BT.State Run(EnemyBase enemy)
    {
        foreach (INode child in children) {
            var result = child.Run(enemy);
            if (result == BT.State.Sucess) {
                return BT.State.Sucess;
            }
        }
        return BT.State.Failure;
    }
}
