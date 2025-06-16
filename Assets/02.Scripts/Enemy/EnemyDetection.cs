using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection :MonoBehaviour
{
    public enum AlertState
    {
        Calm,
        Suspicious,
        Alert
    }
    AlertState state;
    private float alertLevel;
    [SerializeField]
    private LayerMask obstacleMask;
    private GameObject target; //아직 모르겠지만 플레이어 스크립트로 변경 필요
    public GameObject Target { get { return target; } }
    public AlertState State {  get { return state; } }
    [SerializeField]
    private LayerMask targetMask;

    private float sightRange;
    private float sightAngle;
    private float soundRange;
    private float attackRange;
    public EnemyDetection(AlertState alertState)
    {
        state = AlertState.Calm;
        alertLevel = 0;
    }
    public void Init(float sight,float angle,float sound,float attack)
    {
        sightRange = sight;
        attackRange = attack;
        soundRange = sound;
        sightAngle = angle;
    }
    public void SeeTarget()
    {
        alertLevel = 200;
    }
    public void addAlert(float val)
    {
        
        alertLevel += val*Time.deltaTime;
      
       

    }
    public void Update()
    {
        alertLevel-=Time.deltaTime;
        if (alertLevel >= 100)
            state = AlertState.Alert;
        else if (alertLevel >= 60)
            state = AlertState.Suspicious;
        else
        {
            state = AlertState.Calm;
        }
    }
    public bool FindTarget()
    {
        Debug.Log("탐색");
        Collider[] hits = Physics.OverlapSphere(transform.position, sightRange, targetMask);
        Debug.Log(hits.Length);
        if (hits.Length == 0)
            return false;
        foreach (var hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= sightAngle * 0.5f)
            {
                float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    Debug.Log("시야 내 타겟 감지 성공 ");
                    target = hit.gameObject;
                    return true;
                }
                else
                {
                    Debug.Log("벽에 가려짐");
                }
            }
        }
       
        return false;

    }
    public bool ListenTarget()
    {
        Debug.Log("탐색");
        Collider[] hits = Physics.OverlapSphere(transform.position, soundRange, targetMask);
        Debug.Log(hits.Length);
        foreach (var hit in hits)
        {
            
                    target = hit.gameObject;
                    return true;
          
        }
     
        return false;
    }
    public bool CanAttack()
    {
        if (!FindTarget())
            return false;
        if (Vector3.Distance(target.transform.position, transform.position) <= attackRange)
        {

            return true;
        }

        return false;
    }
}
