using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.U2D;
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
    public bool isAttack;
    public Transform mon;
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
        PlayerEvent.OnMonsterHit?.Invoke();
      
        mon.transform.LookAt(detection.Target.transform.position);
        //if (invincibility)
        //return;
        detection.SeeTarget();
        Debug.Log(dmg + " 입음");
        isDamaged=true;
        invincibility=false;
        hp-=dmg;
        if (hp <= 0) {
            Dead();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(MotionE(aniPara.DamagedParaHash));
        }
    }
    public void Dead()
    {
        isDead=true;
        Debug.Log("사망");
        if (DeadC == null)
            DeadC = StartCoroutine(MotionE2(aniPara.DeadParaHash));
        MonsterManager.Instance.Dead(this);
        PlayerEvent.OnKillConfirmed?.Invoke();
    }
    bool invincibility = false;


    IEnumerator MotionE(int para)
 {
        animator.applyRootMotion = true;
        animator.SetBool(aniPara.DamagedParaHash, true);
        animator.SetBool(aniPara.AttackParaHash, false);
        animator.SetBool(para, true);
        invincibility = true;
        controller.Agent.isStopped = true;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return null;
        isAttack = false;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(stateInfo.fullPathHash, 0, 0f);
        while (!stateInfo.IsName("Damaged") || stateInfo.normalizedTime < 1f)
      {

            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        animator.SetBool(para, false);
        isDamaged = false;
        invincibility = false;
        DmdC = null;
        animator.applyRootMotion = false;
        Vector3 pos = mon.position;
        Quaternion rot = mon.rotation;
        mon.SetParent(null);
        transform.position = pos+new Vector3(0,0.1f,0);
        transform.rotation = rot;
        mon.SetParent(gameObject.transform);
      
    }
    Coroutine DmdC;
    public void DamagedMotion()
    {
        //if(DmdC == null) 
       // DmdC = StartCoroutine(MotionE(aniPara.DamagedParaHash));
    }
    Coroutine DeadC;
    IEnumerator MotionE2(int para)
    {
        animator.applyRootMotion = true;
        animator.SetBool(para, true);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Destroy(this.gameObject, 5f);
        yield return null;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsName("Dead")||stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        animator.SetBool(aniPara.DamagedParaHash, false);
        
    }
    public void DeadMotion()
    {
        StopAllCoroutines();
        if (DeadC == null)
            DeadC = StartCoroutine(MotionE2(aniPara.DeadParaHash));
       
    }
 
    void Attack()
    {
        Debug.Log("공격");

    }

    IEnumerator AttackE()
    {
        controller.Look(detection.Target.gameObject.transform.position - transform.position);
        animator.applyRootMotion = true;
        isAttack = true;
        controller.Agent.isStopped=true;
            animator.SetBool(aniPara.AttackParaHash, true);
        Attack();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        controller.Agent.updatePosition = false;
        controller.Agent.updateRotation = false;
        yield return null;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(2.63f);
        controller.Agent.updatePosition = true;
        controller.Agent.updateRotation = true;
        animator.SetBool(aniPara.AttackParaHash, false);
        animator.SetBool(aniPara.RunParaHash, true);
        isAttack = false;
        attack= null;
        animator.applyRootMotion = false;
        Vector3 pos = mon.position;
        Quaternion rot = mon.rotation;
        mon.SetParent(null);
        transform.position = pos + new Vector3(0, 0.1f, 0);
        transform.rotation = rot;
        mon.SetParent(gameObject.transform);
        controller.Agent.isStopped = false;


    }
    Coroutine attack=null;
    public void StartAttack()
    {
        detection.SeeTarget();

  
  
        animator.SetBool(aniPara.RunParaHash, false);
        if(!isAttack)
         attack = StartCoroutine(AttackE());
         
        
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
