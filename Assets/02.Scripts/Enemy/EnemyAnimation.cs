using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAnimation 
{

    [SerializeField] private string walkPara = "isWalk";
    [SerializeField] private string RunPara = "isRun";
    [SerializeField] private string AttackPara = "isAttack";

    public int walkParaHash { get; private set; }
    public int RunParaHash { get; private set; }
    public int AttackParaHash {  get; private set; }
    public void Init()
    {
        walkParaHash=Animator.StringToHash(walkPara);
        RunParaHash=Animator.StringToHash(RunPara);
        AttackParaHash=Animator.StringToHash(AttackPara);
    }
}
