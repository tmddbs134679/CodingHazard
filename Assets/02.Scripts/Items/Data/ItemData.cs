using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemData : ScriptableObject
{
    public abstract ItemType ItemType { get; }
    
    [field: SerializeField] public string DisplayName { get; private set; }
    
    [field: SerializeField] public float Price { get; private set; }
    
    [field: SerializeField] public DroppedItem DroppedPrefab { get; private set; }
}
