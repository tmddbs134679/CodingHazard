using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimationHandler : MonoBehaviour
{
    private Knife _knife;

    private void Awake()
    {
        _knife = GetComponentInChildren<Knife>();
    }

    public void StartSwing()
    {
        _knife.IsAttacking = true;
    }

    public void FinishSwing()
    {
        _knife.IsAttacking = false;
    }
    
    public void Hit()
    {
        _knife.HitWeapon();
    }
}
