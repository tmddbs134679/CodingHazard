using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum BodyPart { Head, Body }
    public BodyPart partType;

    public EnemyBase enemy;
    [SerializeField] GameObject bloodEffectPrefab;
   
    void SpawnBloodEffect(Vector3 pos,Vector3 normal)
    {
        
        GameObject blood = Instantiate(
            bloodEffectPrefab,
            pos,                            
            Quaternion.LookRotation(normal)    
        );

        
        Destroy(blood, 1f);
    }
    public void Damaged(float dmg,Vector3 pos, Vector3 normal)
    {
        float finalDamage = dmg;
        SpawnBloodEffect(pos,normal);
        if (partType == BodyPart.Head)
            finalDamage *= 2;

        enemy.Damaged(finalDamage);
    }
    public void Damaged(float dmg, RaycastHit hit)
    {
        float finalDamage = dmg;
        SpawnBloodEffect(hit.point,hit.normal);
        if (partType == BodyPart.Head)
            finalDamage *= 2;

        enemy.Damaged(finalDamage);
    }
}
