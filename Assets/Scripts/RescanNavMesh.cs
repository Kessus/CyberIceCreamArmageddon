using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//Not used in game; originally intended to refresh enemies' path periodically by rescanning the entire map but caused performance issues
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
