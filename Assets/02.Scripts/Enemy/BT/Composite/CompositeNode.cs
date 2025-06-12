using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : INode
{
    protected List<INode> children;

    public abstract BT.State Run(EnemyBase enemy);
}
