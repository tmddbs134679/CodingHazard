using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class IdleNode : ActionNode
{
    private float wanderTimer = 0f;
    private float wanderRadius = 10f;
    private float wanderTime = 10f;
    private float maxAngle = 60f;
    private Vector3 wanderTarget;

    public override BT.State Run(EnemyBase enemy)
    {
        enemy.animator.SetBool(enemy.aniPara.RunParaHash, false);
        enemy.animator.SetBool(enemy.aniPara.walkParaHash, true);
       
        wanderTimer += Time.deltaTime;

        if (enemy.Controller.HasArrived() || wanderTimer >= wanderTime)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection.y = 0f;
            Vector3 targetPosition = enemy.startPos + randomDirection;

            if (NavMesh.SamplePosition(targetPosition, out var hit, 2.0f, NavMesh.AllAreas))
            {
                wanderTarget = hit.position;
                enemy.Controller.MoveTo(wanderTarget,false);
                wanderTimer = 0f;
            }
        }

        return BT.State.Running;
    }
}