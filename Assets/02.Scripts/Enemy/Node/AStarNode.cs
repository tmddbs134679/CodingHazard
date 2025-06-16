using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode 
{
    public Vector3 pos;
    public Vector3Int coord;
    public bool walkable;
    public bool CanGo;
    public AStarNode(Vector3Int pos, bool walkable, Vector3Int coord)
    {
        this.pos = pos;
        this.walkable = walkable;
        this.coord = coord;
    }
}
public class AStarCalNode
{
    public AStarNode node;
    public AStarCalNode lastnode;
    public int h;
    public int g;
    public int f { get { return h + g; } }
    public AStarCalNode(AStarNode node)
    {
        this.node = node;
    }
}
