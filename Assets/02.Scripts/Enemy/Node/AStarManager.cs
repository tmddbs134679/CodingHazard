using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public Vector3 gridSize;
    [SerializeField] float nodeSpacing = 1;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask obstacleMask;
    public float raycastHeight = 10f;
    private AStarNode[,,] grid;
    private int gridSizeX, gridSizeY,gridSizeZ;
    void Start()
    {
        makeGrid();
    }
    void makeGrid()
    {
        gridSizeX= Mathf.FloorToInt(gridSize.x/nodeSpacing);
        gridSizeY= Mathf.FloorToInt(gridSize.y/nodeSpacing);
        gridSizeZ = Mathf.FloorToInt(gridSize.z/nodeSpacing);
        grid = new AStarNode[gridSizeX, gridSizeY, gridSizeZ];
        Vector3 Startpos = transform.position-gridSize/2;
        Startpos.y = 0;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++) { 
                    Vector3 point = Startpos + new Vector3(x*nodeSpacing, y*nodeSpacing, z*nodeSpacing);
                    Ray ray = new Ray(point + Vector3.up * raycastHeight,Vector3.down);
                    bool isWalkable = Physics.Raycast(ray, raycastHeight*2f,groundMask);
                    grid[x,y,z]=new AStarNode(point,isWalkable);
                
                }
            }
        }
    }
    public bool findCanGo(int x,int y,int z)
    {
        if(x<0 || y<0 || z<0) return false;
        if(x>=gridSizeX||y>=gridSizeY||z>=gridSizeZ) return false;
        if (!grid[x,y,z].walkable) return false;
        Ray ray = new Ray(grid[x,y,z].pos + Vector3.up * raycastHeight, Vector3.down);
        
        return !Physics.Raycast(ray, raycastHeight * 2f, obstacleMask);
    }
    void OnDrawGizmos()
    {
        if (grid == null) return;

        foreach (var node in grid)
        {
            if(node.walkable)
            Gizmos.DrawCube(node.pos, Vector3.one * (nodeSpacing * 0.9f));
        }
    }
}
