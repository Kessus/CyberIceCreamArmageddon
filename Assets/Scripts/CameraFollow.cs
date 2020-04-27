using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    [Tooltip("Min and Max of camera X transform value")]
    public Vector2 xBoundaries;
    [Tooltip("Min and Max of camera Y transform value")]
    public Vector2 yBoundaries;

    private void Update()
    {
        if(player != null)
            transform.position = new Vector3(Mathf.Clamp(player.position.x, xBoundaries.x, xBoundaries.y), Mathf.Clamp(player.position.y, yBoundaries.x, yBoundaries.y), transform.position.z);
    }
}
