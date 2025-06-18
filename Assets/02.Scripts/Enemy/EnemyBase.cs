using System;
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
    public event Action OnDead;
    private EnemySound sound;
    public EnemySound Sound { get { return sound; } }
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
        sound = GetComponent<EnemySound>();
        
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
        if(detection.Target!=null)
        mon.transform.LookAt(detection.Target.transform.position);
        //if (invincibility)
        //return;
        detection.SeeTarget();
        Debug.Log(dmg + " 입음");
  
        invincibility=false;
     
        hp-=dmg;
        if (hp <= 0) {
            Dead();
        }
        if (isDamaged)
            return;
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
        sound.PlaySound(EnemySound.monSound.Dead);
        MonsterManager.Instance.Dead(this);
        PlayerEvent.OnKillConfirmed?.Invoke();
        OnDead.Invoke();

    }
    bool invincibility = false;


    IEnumerator MotionE(int para)
 {
        Sound.PlaySound(EnemySound.monSound.Damaged);
        isDamaged = true;
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
        controller.Agent.updatePosition = false;
        controller.Agent.updateRotation = false;
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
        controller.Agent.updatePosition = true;
        controller.Agent.updateRotation = true;

    }
    Coroutine DmdC;
    public void DamagedMotion()
    {
        //if(DmdC == null) 
       // DmdC = StartCoroutine(MotionE(aniPara.DamagedParaHash));
    }
    Coroutine DeadC=null;
    IEnumerator MotionE2(int para)
    {
        
        animator.applyRootMotion = true;
        animator.SetBool(para, true);
        controller.Agent.updatePosition = false;
        controller.Agent.updateRotation = false;
        controller.Agent.isStopped=true;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Destroy(this.gameObject, 5f);
        yield return null;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

         yield return new WaitForSeconds(3.0f);
           
       
        Debug.Log("삭제 처리 확인");
        sound.StopMusic();
        Component[] components = GetComponents<Component>();
        foreach (Component comp in components)
        {
            if (!(comp is Transform))
            {
                Destroy(comp);
            }
        }

    }
    public void DeadMotion()
    {
        //StopAllCoroutines();
        if (DeadC == null)
            DeadC = StartCoroutine(MotionE2(aniPara.DeadParaHash));
       
    }
 
    void Attack()
    {
        Debug.Log("공격");

    }

    IEnumerator AttackE()
    {
        Vector3 dir = (detection.Target.transform.position - transform.position).normalized;
        dir.y = 0;
       
        transform.rotation = Quaternion.LookRotation(dir);
        animator.applyRootMotion = true;
        isAttack = true;
            animator.SetBool(aniPara.AttackParaHash, true);
        animator.SetBool(aniPara.RunParaHash, false);
       
        Attack();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
      
        yield return null;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(0.5f);
        Sound.PlaySound(EnemySound.monSound.Attack);
        yield return new WaitForSeconds(2.0f);
        animator.SetBool(aniPara.AttackParaHash, false);
        animator.SetBool(aniPara.RunParaHash, true);
        yield return new WaitForSeconds(0.1f);

        controller.StartMove();
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
