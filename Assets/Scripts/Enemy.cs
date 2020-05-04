using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{

    public float speed = 1f;
    public float nextWaypointDistance = 3f;
    public float playerShootDistance = 8.0f;
    public GameObject healthBar = null;
    public bool CanBeHijacked { get
        {
            Damage damageScript = GetComponent<Damage>();
            return (float)(damageScript.bodyHealth) / damageScript.maxBodyHealth <= 0.5f;
        }
    }

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;
    private bool isFacingLeft = false;
    private bool canShoot = false;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        InvokeRepeating("ShootAtPlayer", Random.Range(1.0f, 3.0f), 2.0f);

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

    private void ShootAtPlayer()
    {
        if(canShoot)
            GetComponentInChildren<Weapon>().UseWeapon();
    }

    private void OnDisable()
    {
        CancelInvoke();

        GetComponent<Seeker>().enabled = false;

        HandMovement[] handMovementScripts = gameObject.GetComponentsInChildren<HandMovement>();
        foreach (HandMovement movementScript in handMovementScripts)
        {
            movementScript.aimTowardsPlayer = false;
        }
        Destroy(healthBar);
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
        if (Camera.main.GetComponent<CameraFollow>().player == null)
            return;
        Vector3 playerPosition = Camera.main.GetComponent<CameraFollow>().player.position;
        if (playerPosition.x > transform.position.x && isFacingLeft)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            isFacingLeft = false;
        }
        else if (playerPosition.x < transform.position.x && !isFacingLeft)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            isFacingLeft = true;
        }

        RaycastHit2D enemyLineOfSight =  Physics2D.Linecast(transform.position, playerPosition, 1 << LayerMask.NameToLayer("Ground"));

        if ((playerPosition - transform.position).magnitude > playerShootDistance || enemyLineOfSight.collider != null)
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
            if (direction.y != 1.0f)
            {
                Jumping jumpScript = gameObject.GetComponent<Jumping>();
                if (direction.y > 0.35f)
                {
                    jumpScript.TryJump();
                }
                else if (direction.y < -0.35f)
                {
                    StartCoroutine(jumpScript.TryJumpOffPlatform());
                }
            }

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            canShoot = false;
        }
        else
        {
            canShoot = true;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
        
    }


}
