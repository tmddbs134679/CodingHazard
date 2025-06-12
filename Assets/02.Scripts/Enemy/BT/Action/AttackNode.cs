using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : ActionNode
{
    public override BT.State Run(EnemyBase enemy)
    {
        enemy.StartAttack();
        return BT.State.Sucess;
    }

   
}
//이것보단 데코레이터를 사용하는 방향으로...
public class StopAttackNode : ActionNode
{
    public override BT.State Run(EnemyBase enemy)
    {
        enemy.StopAttack();
        return BT.State.Failure;
    }


}
