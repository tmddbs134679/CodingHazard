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

    private IInteractable currentTargetItem;

    public float detectionRadius = 10f;
    private Vector3 playerPos;
    private HashSet<IDetectable> preDetectedItems = new();

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
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    // Fake Null 상태로... 이중 확인
                    if (currentTargetItem != null && currentTargetItem == null)
                    {
                        currentTargetItem = null;
                    }
                    
                    if (interactable.IsLockInteract)
                        return;

                    //SetItemText(item);
                    PlayerEvent.OnItemCheck.Invoke(interactable);

                    if (currentTargetItem != interactable)
                    {
                        currentTargetItem = interactable;
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
        HashSet<IDetectable> curDetectedItems = new();
        
        foreach (Collider col in detectedColliders)
        {
            if (col.TryGetComponent(out IDetectable triggerable))
            {
                if (triggerable.IsLockDetect)
                    return;
                
                curDetectedItems.Add(triggerable);

                if (!preDetectedItems.Contains(triggerable))
                {
                    triggerable.Enter();
                }
            }
        }

        foreach (var preItem in preDetectedItems)
        {
            if (!curDetectedItems.Contains(preItem))
            {
                preItem.Exit();
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
            currentTargetItem.Interact();
            AudioManager.Instance.PlayAudio(AudioID.PlayerInteract, 0.2f);
            itemText.gameObject.SetActive(false);
        }
    }
}
