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
        
        Debug.Log(1);
        
        if (!isShootable)
        {
            Debug.Log(2);
            return;
        }

        if (IsAttacking)
        {
            Debug.Log(3);
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

    public void HitWeapon()
    {
        bool hitCheck = false;
        Vector3 origin = transform.position + transform.forward * 1.2f;
        Collider[] hits = Physics.OverlapSphere(origin, attackRange, hitLayer);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<HitBox>(out var enemy))
            {
                Vector3 hitPoint = hit.ClosestPoint(origin);
                Vector3 normal = (hitPoint - origin).normalized;
                AudioManager.Instance.PlayAudio(AudioID.AxeHit,0.2f); //맞았을때 
                enemy.Damaged(damage, hitPoint, normal);
                hitCheck = true;
                Debug.Log("칼 타격 성공");
            }
        }

        if (!hitCheck)
        {
            AudioManager.Instance.PlayAudio(AudioID.AxeAttack, 0.4f);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 1.2f, attackRange);

    }




}
