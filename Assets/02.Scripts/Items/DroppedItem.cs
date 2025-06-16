using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public ItemData ItemData => itemData;
    

    [SerializeField] private ItemData itemData;
    

    private Outline _outline;
    

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }
    

    public void InitItemData(ItemData itemData)
    {
        this.itemData = itemData;
    }
    
    

    public void ToggleOutline(bool state)
    {
        if (_outline == null) return;
        _outline.enabled = state;
    }
    
    
    public void OnInteract()
    {
        ToggleOutline(false);
        
        switch (ItemData.ItemType)
        {
            case ItemType.Collect :
                StageManager.Instance.UpdateObjectives(this);
                break;
            default:
                //Swap
                break;
        }
    }
}
