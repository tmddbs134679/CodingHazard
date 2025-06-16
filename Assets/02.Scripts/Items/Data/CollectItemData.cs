using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "collectItemData", menuName = "Scriptable Objects/Data/Item Data/Collect")]
public class CollectItemData : ItemData
{
    public override ItemType ItemType => ItemType.Collect;
    
    
}
