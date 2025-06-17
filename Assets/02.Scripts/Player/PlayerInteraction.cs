using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactableDistance = 3f;
    public LayerMask interactableLayer;
    public float checkRate = 0.05f;
    private float lastCheckTime;

    private DroppedItem currentTargetItem;

    public float detectionRadius = 10f;
    private Vector3 playerPos;
    private HashSet<DroppedItem> preDetectedItems = new();

    public TextMeshProUGUI itemText;

    private void Start()
    {
        StartCoroutine(ItemDetectorLoop());
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            
            Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            
            Debug.DrawRay(ray.origin, ray.direction * interactableDistance, Color.red, 0.1f);
            
            if (Physics.Raycast(ray, out hit, interactableDistance, interactableLayer))
            {
                DroppedItem item = hit.collider.GetComponent<DroppedItem>();

                
                if (item != null)
                {
                    // Fake Null 상태로... 이중 확인
                    if (currentTargetItem != null && currentTargetItem == null)
                    {
                        currentTargetItem = null;
                    }
                    
                    if (item.IsLockInteract)
                        return;

                    //SetItemText(item);
                    PlayerEvent.OnItemCheck.Invoke(item);

                    if (currentTargetItem != item)
                    {
                        currentTargetItem = item;
                    }
                    return;
                }
            }
            
            if (currentTargetItem != null)
            {
                currentTargetItem = null;
                itemText.gameObject.SetActive(false);
            }
        }
    }
    
    private IEnumerator ItemDetectorLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);
        
        while (true)
        {
            DetectItems();
            
            yield return wait;
        }
    }

    private void DetectItems()
    {
        playerPos = gameObject.transform.position;
        
        Collider[] detectedColliders = Physics.OverlapSphere(playerPos, detectionRadius, interactableLayer);
        HashSet<DroppedItem> curDetectedItems = new();
        
        foreach (Collider col in detectedColliders)
        {
            if (col.TryGetComponent(out DroppedItem item))
            {
                if (item.IsLockInteract)
                    return;
                
                curDetectedItems.Add(item);

                if (!preDetectedItems.Contains(item))
                {
                    item.ToggleOutline(true);
                }
            }
        }

        foreach (var preItem in preDetectedItems)
        {
            if (!curDetectedItems.Contains(preItem))
            {
                preItem.ToggleOutline(false);
            }
        }

        preDetectedItems = curDetectedItems;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerPos, detectionRadius);
    }

    
    public void OnInteractInput()
    {
        if (currentTargetItem != null)
        {
            currentTargetItem.OnInteract();
            itemText.gameObject.SetActive(false);
        }
            
    }
}
