using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT : MonoBehaviour
{
    public enum State { Sucess,Failure,Running
    }
    
    protected INode root;
   
    public void Init()
    {

    }
    public virtual void MakeBT()
    {
        root =  new IdleNode();
    }
    public void StartBT(EnemyBase enemy)
    {
        StartCoroutine(Run( enemy));
    }
    IEnumerator Run(EnemyBase enemy)
    {
        while (root != null)
        {
            root.Run(enemy);
            yield return null;
        }
    }
}
