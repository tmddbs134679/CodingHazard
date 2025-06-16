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
    [SerializeField]
    public EnemyDetection detection;
    bool isDamaged;
    private bool isAttack;
    public bool IsAttack {  get { return isAttack; } }
    public bool IsDamaged { set { isDamaged = value; }  get { return isDamaged; } }
    bool isDead;
    public bool IsDead { get { return isDead; } }

    public EnemyController Controller {  get { return controller; } }
    public EnemyStatus Status { get { return status; } }
    public Animator animator;

    private BT bt;

    public Vector3 startPos;

    float hp;
   
    void Awake()
    {
        controller = GetComponent<EnemyController>();
        animator = GetComponentInChildren<Animator>();
        bt = GetComponent<BT>();
        startPos = transform.position;
        detection = GetComponent<EnemyDetection>();
        MonsterManager.Instance.AddMon(this);
        
    }
    private void Start()
    {
        aniPara.Init();
        controller.Init(status.MoveSpeed);
        bt.MakeBT();
        bt.StartBT(this);
        detection.Init(status.SightRange, status.SightAngle, status.SoundRange, status.AttackRange);
        hp=status.HP;
    }
    public void Damaged(float dmg)
    {
        if (invincibility)
            return;
        detection.SeeTarget();
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
       MonsterManager.Instance.Dead(this);
        
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
 
    void Attack()
    {
        Debug.Log("공격");

    }
    IEnumerator AttackE()
    {
        
            isAttack = true;
            animator.SetBool(aniPara.AttackParaHash, true);
            Attack();
           
           
            yield return new WaitForSeconds(status.AttackCoolTime);
            animator.SetBool(aniPara.AttackParaHash, false);
            isAttack = false;
        attack= null;
    }
    Coroutine attack=null;
    public void StartAttack()
    {
        detection.SeeTarget();
        controller.Look(detection.Target.gameObject.transform.position-transform.position);
        animator.SetBool(aniPara.AttackParaHash, true);
        animator.SetBool(aniPara.RunParaHash, false);
        if (attack == null) {
            attack = StartCoroutine(AttackE());
          }
        
    }
 
    void OnDrawGizmos()
    {
        if (status == null) return;

        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, status.SightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, status.AttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, status.SoundRange);


        Vector3 l= Quaternion.Euler(0, status.SightAngle * 0.5f, 0) * transform.forward;
        Vector3 r = Quaternion.Euler(0, -status.SightAngle * 0.5f, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + l * status.SightRange);
        Gizmos.DrawLine(transform.position, transform.position + r * status.SightRange);
    }
    public void Update()
    {
       // if (Input.GetKeyDown(KeyCode.A)) {
       //     Damaged(5);
      //  }
    }
}
