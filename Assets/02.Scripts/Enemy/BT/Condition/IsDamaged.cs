using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDamaged : ConditionNode
{
    public override BT.State Run(EnemyBase enemy)
    {
        if (enemy.IsDamaged)
            return BT.State.Sucess;
        else 
            return BT.State.Failure;
    }

}
public class IsDeadNode : ConditionNode
{
    public override BT.State Run(EnemyBase enemy)
    {
        if (enemy.IsDead)
            return BT.State.Sucess;
        else
            return BT.State.Failure;
    }

}
public class DamagedNode : ActionNode
{
    public override BT.State Run(EnemyBase enemy)
    {
        enemy.DamagedMotion();
        return BT.State.Sucess;
    }
}
public class DeadNode : ActionNode
{
    public override BT.State Run(EnemyBase enemy)
    {
       // enemy.DeadMotion();
        return BT.State.Sucess;
    }
}
