using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    public bool IsAttacking { get; set; }

    [Header("Setting")]
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private LayerMask hitLayer; //적layer


    public override void Fire()
    {
        base.Fire();
        if (!isShootable)
        {
            return;
        }

        if (IsAttacking)
        {
            return;
        }

        //StartCoroutine(AttackRoutine());

        //칼 애니메이션

        if (!IsAttacking)
        {
            PlayAttackAnimation(false);
        }
    }

    private void Update()
    {
        base.Update();
       
    }


    private IEnumerator AttackRoutine()
    {
        IsAttacking = true;

        PlayAttackAnimation(false);
        yield return new WaitForSeconds(attackDelay);
        HitWeapon();

        yield return new WaitForSeconds(fireRate); // 광클하면 애니메이션 이상해짐

        IsAttacking = false;
    }


    public void HitWeapon()
    {
        Vector3 origin = transform.position + transform.forward * 1.2f;
        Collider[] hits = Physics.OverlapSphere(origin, attackRange, hitLayer);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<HitBox>(out var enemy))
            {
                Vector3 hitPoint = hit.ClosestPoint(origin);
                Vector3 normal = (hitPoint - origin).normalized;

                enemy.Damaged(damage, hitPoint, normal);
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
