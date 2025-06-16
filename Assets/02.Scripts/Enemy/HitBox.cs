using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum BodyPart { Head, Body }
    public BodyPart partType;

    public EnemyBase enemy; 

    public void Damage(float dmg)
    {
        float finalDamage = dmg;

        if (partType == BodyPart.Head)
            finalDamage *= 2;

        enemy.Damaged(finalDamage);
    }
}
