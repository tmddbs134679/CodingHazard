using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
    EnemyBase enemy;
    [SerializeField]
    EnemyName enemyName;
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] bool IsAlerted;
    private bool hasSpawned = false;
    private int targetLayer;

    private void Start()
    {
        targetLayer = LayerMask.NameToLayer(targetLayerName);
    }

    public void Spawn(PlayerCondition con)
    {
        if(hasSpawned)
            return;
        enemy =MonsterManager.Instance.MakeEnemy(enemyName, gameObject.transform);
        hasSpawned = true;
        if (IsAlerted)
        {
         
            enemy.detection.SeeTargetSpawn(con);
        }
       
    }


}
