using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : ConditionNode
{

    public override BT.State Run(EnemyBase enemy)
    {
        throw new System.NotImplementedException();
    }

    
}
public class ListenTargetNode : ConditionNode
{

    public override BT.State Run(EnemyBase enemy)
    {
        throw new System.NotImplementedException();
    }


}
public class LookTargetNode : ConditionNode
{

    public override BT.State Run(EnemyBase enemy)
    {
        Debug.Log("Å½»ö Áß");
        if(enemy.FindTarget())
            return BT.State.Sucess;
        else
            return BT.State.Failure;
    }
    
}
public class canAttackNode : ConditionNode
{

    public override BT.State Run(EnemyBase enemy)
    {
        Debug.Log("°ø°Ý °¡´É Å½»ö Áß");
        if (enemy.CanAttack())
            return BT.State.Sucess;
        else
            return BT.State.Failure;
    }

}