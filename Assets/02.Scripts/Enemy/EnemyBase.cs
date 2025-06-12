using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //[SerializeField]
    private EnemyController controller;
    [SerializeField]
    private EnemyStatus status;
    public EnemyController Controller {  get { return controller; } }
    public EnemyStatus Status { get { return status; } }
    
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

}
