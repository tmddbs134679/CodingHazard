using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBase : MonoBehaviour
{
    //[SerializeField]
    private EnemyController controller;
    [SerializeField]
    private EnemyStatus status;
    public EnemyController Controller {  get { return controller; } }
    public EnemyStatus Status { get { return status; } }
    [SerializeField]
    private LayerMask targetMask;
    private BT bt;
    void Awake()
    {
        controller = GetComponent<EnemyController>();
     
        bt = GetComponent<BT>();
        
    }
    private void Start()
    {
        controller.Init(status.MoveSpeed);
        bt.MakeBT();
        bt.StartBT(this);
    }
    public void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, status.SightRange, targetMask);
        foreach(var hit in hits)
        {
            Vector3 dir = (hit.transform.position-transform.position).normalized;
           
            //if(Angle>angle*0.5f)
                //continue;

        }
    }
}
