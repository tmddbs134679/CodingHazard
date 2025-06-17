using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public Condition hp;
    public Condition stamina;


    private void Awake()
    {
        hp.type = Condition.ConditionType.HP;
        stamina.type = Condition.ConditionType.Stamina;
    }
    private void Start()
    {
        hp.CurValue = hp.maxValue = 100f;
        stamina.CurValue = stamina.maxValue = 100f;

        StartCoroutine(RecoverCondition());
    }

    private void Update()
    {
        // Dead
        if (hp.CurValue <= 0f)
        {
            Debug.Log("Player is Dead.");
        }
    }
    
    private IEnumerator RecoverCondition()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (hp.CurValue < hp.maxValue)
            {
                AddHp(1f);
            }
            
            if (stamina.CurValue < stamina.maxValue)
            {
                AddStamina(5f); // 초당 5 회복
            }
        }
    }

    public void AddHp(float amount)
    {
        hp.CurValue = Mathf.Min(hp.curValue + amount, hp.maxValue);
    }
    
    public void TakeDamage(float amount)
    {
        hp.CurValue = Mathf.Max(hp.curValue - amount, 0f);
        PlayerEvent.OnTakeDamaged?.Invoke();
    }

    public void AddStamina(float amount)
    {
        stamina.CurValue = Mathf.Min(stamina.CurValue + amount, stamina.maxValue);
    }

    public void UseStamina(float amount)
    {
        if (stamina.CurValue >= 0f)
        {
            stamina.CurValue -= amount * Time.deltaTime;
        }
    }
}
