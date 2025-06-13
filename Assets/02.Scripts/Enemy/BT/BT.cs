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
        SequenceNode attack = new SequenceNode(new List<INode>{ new canAttackNode(), new AttackNode() });
        SelectorNode attackF=new SelectorNode( new List<INode> {attack,new StopAttackNode() });
        SequenceNode find = new SequenceNode(new List<INode> { new LookTargetNode(),new ChaseNode() });
        root = new SelectorNode(new List<INode> { attackF, find,new IdleNode() });
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
