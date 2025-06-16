using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshManager : Singleton<NavMeshManager>
{
    private NavMeshSurface nms;
    private void Awake()
    {
        nms = GetComponent<NavMeshSurface>();
    }
    private void Start()
    {
        nms.BuildNavMesh();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
