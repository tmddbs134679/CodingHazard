using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyName { normalZombie }
public class MonsterManager : Singleton<MonsterManager>
{
    List<EnemyBase> monsters=new List<EnemyBase> ();
    [SerializeField]
    List<EnemyBase> MonsterPres = new List<EnemyBase> ();
    int monNum;
    int suspiciousNum;
    int elertNum;
    public int MonNum { get { return monNum; } }
 

    protected override void Awake()
    {
        base.Awake();
     
    }
    public void AddMon(EnemyBase a)
    {
        monsters.Add(a);
        monNum++;
    }
    public EnemyBase MakeEnemy(EnemyName enemyName, Transform pos)
    {
        EnemyBase mon=null;
        switch (enemyName)
        {
            case EnemyName.normalZombie:
                mon = Instantiate(MonsterPres[(int)enemyName], pos.position,pos.rotation);
                break;
        }
        return mon;
    }
    public EnemyDetection.AlertState nowState()
    {
        if(elertNum>0)
            return EnemyDetection.AlertState.Alert;
        if (suspiciousNum > 0)
            return EnemyDetection.AlertState.Suspicious;
        else
            return EnemyDetection.AlertState.Calm;
    }
    public void Dead(EnemyBase mon) {
        monsters.Remove(mon);
        monNum--;
    }
    public void Suspicious(bool a)
    {
        if (a) 
        suspiciousNum++;
        else suspiciousNum--;
    }
    public void Elert(bool a)
    {
        if (a)
        elertNum++;
        else
            elertNum--;
    }
}
