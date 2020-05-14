using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Min and Max of camera X transform value")]
    public Vector2 xBoundaries;
    [Tooltip("Min and Max of camera Y transform value")]
    public Vector2 yBoundaries;

    private void Update()
    {
        if(Player.playerObject != null)
            transform.position = new Vector3(Mathf.Clamp(Player.playerObject.transform.position.x, xBoundaries.x, xBoundaries.y), Mathf.Clamp(Player.playerObject.transform.position.y, yBoundaries.x, yBoundaries.y), transform.position.z);
    }
}
