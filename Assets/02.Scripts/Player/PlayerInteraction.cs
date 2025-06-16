using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactableDistance = 3f;
    public LayerMask interactableLayer;
    public float checkRate = 0.05f;
    private float lastCheckTime;

    private DroppedItem currentTargetItem;

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
                    
                    if (!item.IsLockInteract)
                        return;
                    
                    if (currentTargetItem != item)
                    {
                        currentTargetItem?.ToggleOutline(false);
                        currentTargetItem = item;
                        currentTargetItem.ToggleOutline(true);
                    }
                    return;
                }
            }
            
            if (currentTargetItem != null)
            {
                currentTargetItem.ToggleOutline(false);
                currentTargetItem = null;
            }
        }
    }
    
    public void OnInteractInput()
    {
        if (currentTargetItem != null)
            currentTargetItem.OnInteract();
    }
}
