using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{

    [Header("Setting")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private LayerMask hitLayer; //적layer


    private bool isAttacking = false;

    public override void Fire()
    {
        base.Fire();
        if (!isShootable)
        {
            return;
        }

        StartCoroutine(AttackRoutine());

        //칼 애니메이션


    }

    private void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }


    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        PlayAttackAnimation(false);
        yield return new WaitForSeconds(attackDelay);
        HitWeapon();

        yield return new WaitForSeconds(fireRate); // 광클하면 애니메이션 이상해짐

        isAttacking = false;


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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 1.2f, attackRange);

    }




}
