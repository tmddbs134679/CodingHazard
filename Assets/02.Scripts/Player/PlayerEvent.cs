using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerEvent 
{
    public static Action OnAttack;
    public static Action OnJump;
    public static Action<bool> OnSprint;
    public static Action<int> Swap;
    public static Action<bool> Aiming;
    public static Action<float> OnHpChanged;
    public static Action<float> OnStaminaChanged;
    public static Action OnKillConfirmed;
    public static Action OnMonsterHit;
    public static Action<bool> OnDetectMonster;

}
