using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{

    public float speed = 1f;
    public float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (Camera.main.GetComponent<CameraFollow>().player != null)
            {
                seeker.StartPath(rb.position, Camera.main.GetComponent<CameraFollow>().player.position, OnPathComplete);
            }
        }
            
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (path == null)
            return;
        else if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = new Vector2(direction.x * speed, rb.velocity.y);

        rb.velocity = force;
        if(direction.y != 1.0f)
        {
            Jumping jumpScript = gameObject.GetComponent<Jumping>();
            if (direction.y > 0.65f)
            {
                jumpScript.TryJump();
            }
            else if (direction.y < -0.65f)
            {
                StartCoroutine(jumpScript.TryJumpOffPlatform());
            }
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }


}
