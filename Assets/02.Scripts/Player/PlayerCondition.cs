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

        StartCoroutine(RecoverCondition());
    }

    private void Update()
    {
        // Dead
        if (hp.curValue <= 0f)
        {
            Debug.Log("Player is Dead.");
        }
    }
    
    private IEnumerator RecoverCondition()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (hp.curValue < hp.maxValue)
            {
                AddHp(5f);
            }
            
            if (stamina.curValue < stamina.maxValue)
            {
                AddStamina(5f); // 초당 5 회복
            }
        }
    }

    public void AddHp(float amount)
    {
        hp.curValue = Mathf.Min(hp.curValue + amount, hp.maxValue);
    }
    
    public void TakeDamage(float amount)
    {
        hp.curValue = Mathf.Max(hp.curValue - amount, 0f);
        PlayerEvent.OnHpChanged?.Invoke(hp.curValue);
    
    }

    public void AddStamina(float amount)
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
