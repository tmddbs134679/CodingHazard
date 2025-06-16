using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArm : MonoBehaviour
{
    [SerializeField] EnemyBase enemy;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ãæµ¹"+other);
        if(!enemy.IsAttack)
            return;
        PlayerCondition player = other.GetComponent<PlayerCondition>();
        if (player!=null)
        {
            player.TakeDamage(enemy.Status.DMG);
        }
    }
}
