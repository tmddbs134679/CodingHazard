using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class AStarManager : Singleton<AStarManager>
{
    public Vector3 gridSize;
    [SerializeField] int nodeSpacing = 1;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask obstacleMask;
    public float raycastHeight = 10f;
    private AStarNode[,,] grid;
    private int gridSizeX, gridSizeY,gridSizeZ;
    public Transform startP;
    public Transform endP;
    Vector3Int startPos;
    void Start()
    {
        makeGrid();
        Vector3Int start = Vector3Int.FloorToInt(startP.position);

        Vector3Int end = Vector3Int.FloorToInt(endP.position);



        AstarBase(start, end);
    }
    void makeGrid()
    {
        gridSizeX= Mathf.FloorToInt(gridSize.x/nodeSpacing);
        gridSizeY= Mathf.FloorToInt(gridSize.y/nodeSpacing);
        gridSizeZ = Mathf.FloorToInt(gridSize.z/nodeSpacing);
        grid = new AStarNode[gridSizeX, gridSizeY, gridSizeZ];
        startPos = Vector3Int.FloorToInt(transform.position-gridSize/2);
     

        float offsetAmount = nodeSpacing * 0.5f;
        Vector3[] offsets = new Vector3[]
                   {
                    Vector3.zero,                                           // 중심
                     new Vector3(-offsetAmount, 0,  offsetAmount),           // 좌상
                     new Vector3( offsetAmount, 0,  offsetAmount),           // 우상
                        new Vector3(-offsetAmount, 0, -offsetAmount),           // 좌하
                    new Vector3( offsetAmount, 0, -offsetAmount)            // 우하
                   };
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++) { 
                    Vector3Int point = startPos + new Vector3Int(x*nodeSpacing, y*nodeSpacing, z*nodeSpacing);
             
                    Vector3 baseRayOrigin = point + Vector3.up * raycastHeight;
                   
                    bool isWalkable = true;
                    foreach (Vector3 offset in offsets)
                    {
                        Ray ray = new Ray(baseRayOrigin + offset, Vector3.down);
                        if (!Physics.Raycast(ray, raycastHeight * 2f, groundMask))
                        {
                            isWalkable = false;
                            break;
                        }
                    }
                       grid[x,y,z]=new AStarNode(point,isWalkable,new Vector3Int(x,y,z));
                 
               
                }
            }
        }
    }
    void TryAddNode(int nx, int ny, int nz, int cost,
                AStarCalNode current, Vector3Int target,
                List<AStarCalNode> open, List<AStarNode> close)
    {
        // 범위 검사
        if (nx < 0 || ny < 0 || nz < 0 || nx >= gridSizeX || ny >= gridSizeY || nz >= gridSizeZ)
            return;

        // 못 가는 곳
        if (!findCanGo(nx, ny, nz)) return;

        AStarNode nextNode = grid[nx, ny, nz];

        if (close.Contains(nextNode)) return;

        AStarCalNode newNode = makeCalNode(new Vector3Int(nx, ny, nz));
        newNode.g = current.g + cost;
        newNode.h = (Mathf.Abs(target.x - nx) + Mathf.Abs(target.y - ny) + Mathf.Abs(target.z - nz)) * 10;
    
        newNode.lastnode = current;

        // open 리스트에서 같은 노드 찾기
        AStarCalNode existing = open.Find(n => n.node == nextNode);
        if (existing != null)
        {
            if (newNode.g < existing.g)
            {
                existing.g = newNode.g;
             
                existing.lastnode = current;
            }
        }
        else
        {
            open.Add(newNode);
        }
    }
    public bool findCanGo(int x,int y,int z)
    {
    

        if (x<0 || y<0 || z<0) return false;
        if(x>=gridSizeX||y>=gridSizeY||z>=gridSizeZ) return false;
        if (!grid[x,y,z].walkable) return false;
       
        Vector3 baseRayOrigin = grid[x, y, z].pos-new Vector3(0.1f,0.1f,0.1f);
        bool isCanGo = true;
        float offsetAmount = nodeSpacing * 0.5f;

        Vector3[] offsets = new Vector3[]
                   {
                    Vector3.zero,                                           // 중심
                     new Vector3(-offsetAmount, 0,  offsetAmount),           // 좌상
                     new Vector3( offsetAmount, 0,  offsetAmount),           // 우상
                        new Vector3(-offsetAmount, 0, -offsetAmount),           // 좌하
                    new Vector3( offsetAmount, 0, -offsetAmount)            // 우하
                   };
        foreach (Vector3 offset in offsets)
        {
            Ray ray = new Ray(baseRayOrigin + offset, Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (raycastHeight * 2f), Color.red, 5f);
            if (Physics.Raycast(ray, raycastHeight * 2f, obstacleMask))
            {
                isCanGo = false;
                break;
            }
        }
        //Debug.Log(grid[x, y, z].pos+""+isCanGo);
        return isCanGo;
    }
    int[] wasddx = {1,0,-1,0 };
    int[] wasddz = {0,1,0,-1};
    int[] qezcdx= {1,1,-1,-1};
    int[] qezcdz= {1,-1,1,-1};
    int[] dy = { 1, -1 };
   
    public void AstarBase(Vector3Int start, Vector3Int target)
    {
        List<AStarCalNode> open = new List<AStarCalNode>();
        List<AStarNode> close = new List<AStarNode>();
        Debug.Log(start);
        Debug.Log(target);
        open.Add(makeCalNode(start-startPos));
        bool isFind = false;
        AStarCalNode current = null;
      
        target = target - startPos;
        int cnt = 0;
        while (open.Count > 0) {
            cnt++;
            if (cnt > 100000)
            {
                Debug.LogError("무한루프");
                break;
            }
        
            current = null;
            foreach (AStarCalNode node in open) {
                if (current == null)
                    current = node;
                else
                {
                    if(node.f<current.f)
                        current = node;
                }
            }
            int x = current.node.coord.x;
            int y = current.node.coord.y;
            int z = current.node.coord.z; 

            if (target.x == x && target.y == y && target.z == z) {
                isFind = true;
                Debug.Log("찾음"+current.node.coord);
                break;
            }
            for (int i = 0; i < 4; i++)
                TryAddNode(x + wasddx[i], y, z + wasddz[i], 10, current, target, open, close);

            for (int i = 0; i < 4; i++)
                TryAddNode(x + qezcdx[i], y, z + qezcdz[i], 14, current, target, open, close);

            // 위아래
            for (int i = 0; i < 2; i++)
                TryAddNode(x, y + dy[i], z, 10, current, target, open, close);

            // 위/아래 + 수평
            for (int i = 0; i < 8; i++)
                TryAddNode(x + wasddx[i % 4], y + dy[i / 4], z + wasddz[i % 4], 14, current, target, open, close);

            // 위/아래 + 대각선
            for (int i = 0; i < 8; i++)
                TryAddNode(x + qezcdx[i % 4], y + dy[i / 4], z + qezcdz[i % 4], 17, current, target, open, close);
               close.Add(grid[x,y,z]);
            open.Remove(current);
            
            
        }
        if(!isFind)
            return;
        makeRoute(current);



    }

  
    public void makeRoute(AStarCalNode a)
    {
        AStarCalNode nowNode = a;
        List<AStarNode> nodes = new List<AStarNode>();
        int cnt = 0;
        while (nowNode.lastnode != null) {
            cnt++;
            if (cnt > 100)
            {
                Debug.LogError("무한루프");
                break;
            }
            nodes.Add(nowNode.node);
            nowNode = nowNode.lastnode;
            Debug.Log(nowNode.node.pos);
        }
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 from = nodes[i].pos;
            Vector3 to = nodes[i + 1].pos;
            Debug.DrawLine(from, to, Color.green, 5f); // 5초 동안 유지
        }

    }
    private AStarCalNode makeCalNode(Vector3Int pos)
    {

        AStarCalNode temp = new AStarCalNode(grid[pos.x,pos.y,pos.z]);
        return temp;
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
