﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    public bool aimTowardsPlayer = true;

    // Update is called once per frame
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
        if (targetPosition.y < transform.position.y)
            resultZRotation *= -1;
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, resultZRotation);
    }
}
