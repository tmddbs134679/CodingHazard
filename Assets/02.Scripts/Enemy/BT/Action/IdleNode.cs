using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : ActionNode
{
    Vector3 startpos=new Vector3(5,0,0);
    Vector3 endPos=new Vector3(-5,0,0);
    Vector3 targetPos;
    private bool goToEnd = true;
    public override BT.State Run(EnemyBase enemy)
    {
        enemy.animator.SetBool(enemy.aniPara.walkParaHash,false);
        
        return BT.State.Running;
    }

  
}
