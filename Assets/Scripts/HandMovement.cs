using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allows GameObjects to be rotated either towards the cursor or towards current player position
public class HandMovement : MonoBehaviour
{
    public bool aimTowardsPlayer = true;    //If false, will aim towards mouse cursor
    public Vector2 rotationRange = new Vector2(-90.0f, 90.0f);

    void Update()
    {
        if (InGameUi.IsGamePaused)
            return;

        Vector3 targetPosition;
        if (Player.playerObject == null)
            return;

        Transform playerTransform = Player.playerObject.transform;
        
        if (aimTowardsPlayer)
            targetPosition = playerTransform.position;
        else
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, 0.0f);

        Vector3 lookDirection = Vector3.Normalize(targetPosition - transform.position);
        float resultZRotation = Mathf.Acos(Vector3.Dot(transform.parent.transform.right, lookDirection)) * Mathf.Rad2Deg;
        //Adjusting the angle based on the GameObject's rotation
        if (targetPosition.y < transform.position.y)
            resultZRotation *= -1;
        //Constraining the resulting rotation to specified bounds
        resultZRotation = Mathf.Clamp(resultZRotation, rotationRange.x, rotationRange.y);
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, resultZRotation);
    }
}
