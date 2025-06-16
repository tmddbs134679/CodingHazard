using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum BodyPart { Head, Body }
    public BodyPart partType;

    public EnemyBase enemy;
    [SerializeField] GameObject bloodEffectPrefab;
   
    void SpawnBloodEffect(RaycastHit hit)
    {
        
        GameObject blood = Instantiate(
            bloodEffectPrefab,
            hit.point,                             
            Quaternion.LookRotation(hit.normal)    
        );

        
        Destroy(blood, 1f);
    }
    public void Damage(float dmg, RaycastHit hit)
    {
        float finalDamage = dmg;
        SpawnBloodEffect(hit);
        if (partType == BodyPart.Head)
            finalDamage *= 2;

        enemy.Damaged(finalDamage);
    }
}
