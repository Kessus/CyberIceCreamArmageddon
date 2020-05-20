using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Min and Max of camera X transform value for each stage")]
    public List<Vector2> stageXBoundaries;
    public float minYPosition;
    /*[Tooltip("Min and Max of camera Y transform value")]
    public List<Vector2> stageYBoundaries;*/

    private Vector3 lastCameraPosition;

    private void Start()
    {
        lastCameraPosition = transform.position;
    }

    private void Update()
    {
        if(Player.playerObject != null && stageXBoundaries.Count > SceneGoalManager.goalManager.CurrentStageIndex)
        {
            float minXPosition = Mathf.Min(transform.position.x, stageXBoundaries[SceneGoalManager.goalManager.CurrentStageIndex].x);
            float maxXPosition = stageXBoundaries[SceneGoalManager.goalManager.CurrentStageIndex].y;
            transform.position = new Vector3(Mathf.Clamp(Player.playerObject.transform.position.x, minXPosition, maxXPosition), Mathf.Max(Player.playerObject.transform.position.y, minYPosition), transform.position.z);

        }
    }
}
