using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{

    [Header("Setting")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private LayerMask hitLayer; //적layer

    public override void Fire()
    {
        base.Fire();
        if (!isShootable)
        {
            return;
        }

        //칼 애니메이션


    }
    private void HitWeapon()
    {
        Vector3 origin = transform.position + transform.forward * 1.2f;
        Collider[] hits = Physics.OverlapSphere(origin, attackRange, hitLayer);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyBase>(out var enemy) && !enemy.IsDead)
            {
                enemy.Damaged(damage);
                Debug.Log("칼 타격 성공");
            }
        }
    }




}
