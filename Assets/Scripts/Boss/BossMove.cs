using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : StateMachineBehaviour
{
    public float distanceThreshold = 0.1f;
    private Transform destination;
    private BossBehaviour bossBehaviour;
    private Rigidbody2D rigidBody;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossBehaviour = animator.gameObject.GetComponent<BossBehaviour>();
        rigidBody = animator.gameObject.GetComponent<Rigidbody2D>();
        destination = bossBehaviour.CurrentMoveDestination;
        if (destination.position.x < animator.gameObject.transform.position.x)
            animator.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        else
            animator.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 newPosition = Vector2.MoveTowards(animator.gameObject.transform.position, destination.position, bossBehaviour.movementSpeed * Time.fixedDeltaTime);
        rigidBody.MovePosition(newPosition);
        if ((animator.gameObject.transform.position - destination.position).magnitude <= distanceThreshold)
            animator.SetBool("IsMoving", false);
    }
}
