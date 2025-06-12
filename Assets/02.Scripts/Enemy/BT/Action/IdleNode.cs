using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : ActionNode
{
    Vector3 startpos=new Vector3(5,1,0);
    Vector3 endPos=new Vector3(-5,1,0);
    Vector3 targetPos;
    private bool goToEnd = true;
    public override BT.State Run(EnemyBase enemy)
    {
        Debug.Log("아이들 진행중");
        if (targetPos == new Vector3(0, 0, 0))
        {
            targetPos = endPos;
        }
        Vector3 dir = (targetPos-enemy.transform.position).normalized;
        Debug.Log(targetPos);
        Debug.Log(enemy.transform.position);
        Debug.Log(dir);
        enemy.Controller.Move(dir);
        float distance = Vector3.Distance(targetPos, enemy.transform.position);
        if (distance < 0.1f)
        {
            if(goToEnd)
                targetPos=startpos;
            else 
                targetPos = endPos;
            goToEnd=!goToEnd;
        }
        return BT.State.Running;
    }

  
}
