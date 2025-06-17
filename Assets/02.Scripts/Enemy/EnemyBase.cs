using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
        PlayerEvent.OnMonsterHit?.Invoke();

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
    public Transform spineBone;
    public float bendAmount = 20f;
    public float duration = 0.3f;

    IEnumerator SpineBend(Vector3 attackerPosition)
    {
        Vector3 hitDir = (transform.position - attackerPosition).normalized;
        Vector3 localDir = transform.InverseTransformDirection(hitDir);

        float bendX = Mathf.Clamp(-localDir.z * 30f, -30f, 30f); 
        float bendY = Mathf.Clamp(-localDir.x * 30f, -30f, 30f); 

        Quaternion originalRot = spineBone.localRotation;
        Quaternion targetRot = originalRot * Quaternion.Euler(bendX, bendY, 0);

        float duration = 0.15f;
        float t = 0f;

      
        while (t < duration)
        {
            spineBone.localRotation = Quaternion.Slerp(originalRot, targetRot, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

   
        t = 0f;
        while (t < duration)
        {
            spineBone.localRotation = Quaternion.Slerp(targetRot, originalRot, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
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
