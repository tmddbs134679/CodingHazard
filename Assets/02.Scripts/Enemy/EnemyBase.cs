using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBase : MonoBehaviour
{
    //[SerializeField]
    private EnemyController controller;
    [SerializeField]
    private EnemyStatus status;
    [SerializeField]
    public EnemyAnimation aniPara;
    bool isDamaged;
    public bool IsDamaged { set { isDamaged = value; }  get { return isDamaged; } }
    bool isDead;
    public bool IsDead { get { return isDead; } }

    public EnemyController Controller {  get { return controller; } }
    public EnemyStatus Status { get { return status; } }
    public Animator animator;
    [SerializeField]
    private LayerMask targetMask;
    private BT bt;
    private GameObject target; //아직 모르겠지만 플레이어 스크립트로 변경 필요
    public GameObject Target { get { return target; } }
    public Vector3 startPos;

    float hp;
   
    void Awake()
    {
        controller = GetComponent<EnemyController>();
        animator = GetComponentInChildren<Animator>();
        bt = GetComponent<BT>();
        startPos = transform.position;
        
    }
    private void Start()
    {
        aniPara.Init();
        controller.Init(status.MoveSpeed);
        bt.MakeBT();
        bt.StartBT(this);
        hp=status.HP;
    }
    public void Damaged(float dmg)
    {
        if (invincibility)
            return;
        Debug.Log(dmg + " 입음");
        isDamaged=true;
        invincibility=false;
        hp-=dmg;
        if (hp <= 0) {
            Dead();
        }
    }
    public void Dead()
    {
        isDead=true;
        Debug.Log("사망");
       
        
    }
    bool invincibility = false;
    IEnumerator MotionE(int para)
    {
        Debug.Log("데미지 모션");
        animator.SetBool(para, true);
        invincibility=true;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        yield return null;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsName("Damaged")||stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        animator.SetBool(para, false);
        isDamaged = false;
        invincibility = false;
        DmdC = null;
    }
    Coroutine DmdC;
    public void DamagedMotion()
    {
        if(DmdC == null) 
        DmdC = StartCoroutine(MotionE(aniPara.DamagedParaHash));
    }
    Coroutine DeadC;
    IEnumerator MotionE2(int para)
    {

        animator.SetBool(para, true);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return null;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsName("Dead")||stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        Destroy(this.gameObject);
        animator.SetBool(aniPara.DamagedParaHash, false);
        
    }
    public void DeadMotion()
    {
        if (DeadC == null)
            DeadC = StartCoroutine(MotionE2(aniPara.DeadParaHash));
       
    }
    public bool FindTarget()
    {
        Debug.Log("탐색");
        Collider[] hits = Physics.OverlapSphere(transform.position, status.SightRange, targetMask);
        Debug.Log(hits.Length); 
        foreach(var hit in hits)
        {
            Debug.Log("성공");
            target = hit.gameObject;
            return true;
        }
        target = null;
       return false;
       
    }
    public bool CanAttack()
    {
        if(!FindTarget())
            return false;
        if(Vector3.Distance(target.transform.position,transform.position)<=status.AttackRange)
        {
          
            return true;
        }
      
        return false;
    }
    void Attack()
    {
        Debug.Log("공격");
        animator.SetBool(aniPara.AttackParaHash, true);
    }
    IEnumerator AttackE()
    {
        while (true)
        {
            animator.SetBool(aniPara.AttackParaHash, true);
            Attack();
            animator.SetBool(aniPara.AttackParaHash, false);
            yield return new WaitForSeconds(status.AttackCoolTime);
        }
    }
    Coroutine attack=null;
    public void StartAttack()
    {
        Debug.Log("공격 시행");
        animator.SetBool(aniPara.AttackParaHash, true);
        animator.SetBool(aniPara.RunParaHash, false);
        if (attack == null) {
            attack = StartCoroutine(AttackE());
          }
        
    }
    public void StopAttack()
    {
        Debug.Log("공격 실패");
        animator.SetBool(aniPara.AttackParaHash, false);
      
        if (attack != null) { 
            StopCoroutine(attack);
        }
    }
    void OnDrawGizmos()
    {
        if (status == null) return;

        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, status.SightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, status.AttackRange);
    }
    public void Update()
    {
       // if (Input.GetKeyDown(KeyCode.A)) {
       //     Damaged(5);
      //  }
    }
}
