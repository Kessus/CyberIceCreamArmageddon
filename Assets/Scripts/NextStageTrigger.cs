using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageTrigger : MonoBehaviour
{
    public SceneBlockage controlledBlockage;

    private bool wasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasTriggered || collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        SceneGoalManager.goalManager.BeginNewStage();
        controlledBlockage.SetBlockageActiveState(true);
        wasTriggered = true;
    }
}
