using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode 
{
    public Vector3 pos;
    public bool walkable;
    public bool CanGo;
    public AStarNode(Vector3 pos, bool walkable)
    {
        this.pos = pos;
        this.walkable = walkable;
    }
}
