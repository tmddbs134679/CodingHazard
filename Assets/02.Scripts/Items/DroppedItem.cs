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
        _outline.enabled = state;
    }
    
    
    public void OnInteract()
    {
        switch (ItemData.ItemType)
        {
            case ItemType.Collect :
                StageManager.Instance.CollectItem();
                Destroy(gameObject);
                break;
            default:
                //Swap
                break;
        }
    }
}
