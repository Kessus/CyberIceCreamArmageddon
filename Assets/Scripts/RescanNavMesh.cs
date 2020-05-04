using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RescanNavMesh : MonoBehaviour
{
    private AstarPath pathfinder;
    void Start()
    {
        pathfinder = GetComponent<AstarPath>();
        InvokeRepeating("RescanPath", 0.0f, 0.15f);
    }

    private void RescanPath()
    {
        if (!pathfinder.isScanning)
            pathfinder.Scan();
    }
}
