using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : ActionNode
{
    public override BT.State Run(EnemyBase enemy)
    {
        enemy.Controller.StopMove();
        enemy.StartAttack();
       
        return BT.State.Sucess;
    }

   
}
//이것보단 데코레이터를 사용하는 방향으로...
public class StopAttackNode : ActionNode
{
    public override BT.State Run(EnemyBase enemy)
    {
        if (enemy.IsAttack)
            return BT.State.Sucess;
        else
            return BT.State.Failure;
    }


}
