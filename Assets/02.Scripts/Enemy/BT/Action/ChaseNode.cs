using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : ActionNode
{
    //이거 추적 속도만 변화 시키고 패트롤과 병합 필요... 패트롤 자체를 나눌 필요도 있어보임 
    public override BT.State Run(EnemyBase enemy)
    {
      
        enemy.animator.SetBool(enemy.aniPara.RunParaHash, true);
        enemy.animator.SetBool(enemy.aniPara.walkParaHash, false);
        Debug.Log("추적 진행중");
        Vector3 dir = (enemy.detection.Target.transform.position - enemy.transform.position).normalized;

        Debug.Log(enemy.transform.position);
        Debug.Log(dir);
        Vector3 forward = enemy.transform.forward;
        dir.y = 0;
        forward.y = 0;
         enemy.Controller.Look(dir);
            
        enemy.Controller.MoveTo(enemy.detection.Target.transform.position,true);
        float distance = Vector3.Distance(enemy.detection.Target.transform.position, enemy.transform.position);

        if (distance < 0.1f)
        {
            return BT.State.Sucess;
        }
        
        return BT.State.Sucess;
    }
}
