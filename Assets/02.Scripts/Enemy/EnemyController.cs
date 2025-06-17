using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class EnemyController : MonoBehaviour
{

    private float moveSpeed;
 
    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Init(float _moveSpeed)
    {
        moveSpeed = _moveSpeed;
        agent.speed = moveSpeed;
    }

    public void Look(Vector3 dir)
    {
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);
        }
    }
    public void StopMove()
    {
        agent.ResetPath();
    }
    public bool HasArrived(float stoppingDistance = 0.2f)
    {
        return !Agent.pathPending &&
               Agent.remainingDistance <= stoppingDistance &&
               (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f);
    }
    public void MoveTo(Vector3 targetPosition,bool isRun)
    {
        agent.isStopped = false;
        if (isRun) agent.speed = moveSpeed*2;
        else
        {
            agent.speed = moveSpeed;
        }
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(targetPosition);
        }
    }
}
