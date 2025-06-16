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

            if (Physics.Raycast(ray, out hit, interactableDistance, interactableLayer))
            {
                DroppedItem item = hit.collider.GetComponent<DroppedItem>();

                if (item != null)
                {
                    currentTargetItem = item;
                    
                    if (currentTargetItem != item)
                    {
                        //currentTargetItem?.ToggleOutline(false);
                        currentTargetItem = item;
                        //currentTargetItem.ToggleOutline(true);
                    }
                }
                return;
            }

            /*if (currentTargetItem != null)
            {
                //currentTargetItem.ToggleOutline(false);
                currentTargetItem = null;
            }*/
        }
    }
    
    public void OnInteractInput()
    {
        if (currentTargetItem != null)
            currentTargetItem.OnInteract();
    }
}
