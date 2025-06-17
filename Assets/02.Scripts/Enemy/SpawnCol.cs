using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCol : MonoBehaviour
{
    [SerializeField]
    DropPoint[] dp;
    [SerializeField] private string targetLayerName = "Player";


    private int targetLayer;
    private void Start()
    {
        targetLayer = LayerMask.NameToLayer(targetLayerName);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌");
        if (other.gameObject.layer == targetLayer)
        {
            Debug.Log("소환");
            foreach (DropPoint dp in dp) { 
                dp.Spawn(other.GetComponent<PlayerCondition>());
            }
        }
    }
}
