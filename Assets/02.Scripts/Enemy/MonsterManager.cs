using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    List<EnemyBase> monsters=new List<EnemyBase> ();

    int monNum;
    int suspiciousNum;
    int elertNum;
    public int MonNum { get { return monNum; } }

    protected override void Awake()
    {
        base.Awake();
     
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
    public void Dead() {
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
