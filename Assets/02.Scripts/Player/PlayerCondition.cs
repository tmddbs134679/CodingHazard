using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public Condition hp;
    public Condition stamina;

    private void Start()
    {
        hp.curValue = hp.maxValue = 100f;
        stamina.curValue = stamina.maxValue = 50f;
    }

    private void Update()
    {
        // Dead
        if (hp.curValue <= 0f)
        {
            Debug.Log("Player is Dead.");
        }
    }
    
    public void TakeDamage(float amount)
    {
        hp.curValue = Mathf.Max(hp.curValue - amount, 0f);
    }

    public void RecoverStamina(float amount)
    {
        stamina.curValue = Mathf.Min(stamina.curValue + amount, stamina.maxValue);
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue >= amount)
        {
            stamina.curValue -= amount;
            return true;
        }
        return false;
    }
}
