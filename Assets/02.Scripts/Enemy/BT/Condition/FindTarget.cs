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
       
        if (enemy.detection.ListenTarget())
        {
            enemy.detection.addAlert(5);
           
        }
        if(enemy.detection.State==EnemyDetection.AlertState.Suspicious)
            return BT.State.Sucess;
        else
            return BT.State.Failure;
    }


}
public class LookTargetNode : ConditionNode
{

    public override BT.State Run(EnemyBase enemy)
    {
        
        if (enemy.detection.FindTarget())
        {
            enemy.detection.SeeTarget();
            return BT.State.Sucess;
        }
        if (enemy.detection.State==EnemyDetection.AlertState.Alert)
        {
            
            return BT.State.Sucess;
        }
        else
            return BT.State.Failure;
    }
    
}
public class canAttackNode : ConditionNode
{

    public override BT.State Run(EnemyBase enemy)
    {

        if (enemy.detection.CanAttack())
            return BT.State.Sucess;
        else
            return BT.State.Failure;
    }

}