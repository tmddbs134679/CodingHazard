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
    [SerializeField] private string DamagedPara = "isDamaged";
    [SerializeField] private string DeadPara = "isDead";
    public int walkParaHash { get; private set; }
    public int RunParaHash { get; private set; }
    public int AttackParaHash {  get; private set; }
    public int DamagedParaHash {  get; private set; }
    public int DeadParaHash { get; private set; }
    public void Init()
    {
        walkParaHash=Animator.StringToHash(walkPara);
        RunParaHash=Animator.StringToHash(RunPara);
        AttackParaHash=Animator.StringToHash(AttackPara);
        DamagedParaHash = Animator.StringToHash(DamagedPara);
        DeadParaHash = Animator.StringToHash(DeadPara);
    }
}
