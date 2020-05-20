using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossBehaviour : MonoBehaviour
{
    public Transform CurrentMoveDestination { get; private set; }
    public List<Transform> destinationPoints;
    public float movementSpeed = 5.0f;
    public float stateChangeFrequency = 2.0f;
    public bool IsPoweringUp { get; private set; } = false;
    public bool IsPoweredUp { get; private set; } = false;
    public SpriteRenderer shieldSprite;

    private Queue<Transform> destinationQueue;
    private List<string> availableStates;
    private string lastState = "Wait";
    private Animator animator;

    void Start()
    {
        for (int i = 0; i < destinationPoints.Count; i++)
        {
            destinationQueue.Enqueue(destinationPoints[i]);
        }
        availableStates = new List<string>()
        {
            "Move",
            "Attack",
            "Defend",
            "Wait"
        };
        animator = gameObject.GetComponent<Animator>();

        InvokeRepeating("ChangeState", stateChangeFrequency, stateChangeFrequency);
    }

    private void ChangeState()
    {
        List<string> validStates = availableStates.Where(s => s != lastState).ToList();
        string newState = validStates[Random.Range(0, validStates.Count)];
        Animator animator = gameObject.GetComponent<Animator>();
        switch (newState)
        {
            case "Move":
                CurrentMoveDestination = destinationQueue.Dequeue();
                destinationQueue.Enqueue(CurrentMoveDestination);
                animator.SetBool("IsMoving", true);
                break;
            case "Attack":
                animator.SetTrigger("Attack");
                break;
            case "Defend":
                animator.SetTrigger("Defend");
                break;
            case "Wait":
                break;
        }
    }

    public void PowerUp()
    {
        IsPoweringUp = true;
        IsPoweredUp = true;
        animator.SetTrigger("PowerUp");
    }

    public void DeactivateShield()
    {
        shieldSprite.enabled = false;
    }

    public void ActivateShield()
    {

    }
}
