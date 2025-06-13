using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="enemy",menuName ="enemies/enemy")]
public class EnemyStatus : ScriptableObject
{
    [SerializeField]
    private string name;
    public string Name { get { return name; } }
    [SerializeField]
    private float hp;
    public float HP { get { return hp; } }
    [SerializeField]
    private float dmg;
    public float DMG { get { return dmg; } }
    [SerializeField]
    private float soundRange;
    public float SoundRange { get { return soundRange; } }
    [SerializeField]
    private float sightRange;
    public float SightRange { get {return sightRange; } }
    [SerializeField]
    private float attackRange;
    public float AttackRange { get { return attackRange; } }
    [SerializeField]
    private float sightAngle;
    public float SightAngle { get { return sightAngle; } }
    [SerializeField]
    private float attackCoolTime;
    public float AttackCoolTime { get { return attackCoolTime; } }
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get {return moveSpeed; } }
    [SerializeField]
    private float lostCoolTime;
    public float LostCoolTime { get {return lostCoolTime; } }
}
