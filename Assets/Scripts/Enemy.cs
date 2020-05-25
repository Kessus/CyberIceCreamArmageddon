using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public float playerShootDistance = 8.0f;
    public float runAwayDistance = 0.0f;
    public GameObject healthBar = null;
    public bool CanBeHijacked { get
        {
            Damage damageScript = GetComponent<Damage>();
            return (float)(damageScript.bodyHealth) / damageScript.maxBodyHealth <= 0.5f;
        }
    }
    public bool shouldRunAway = false;
    
    private Path path;
    private int currentWaypoint = 0;
    private float nextWaypointDistance = 3f;
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

        SpawnerManager.mainManager.RegisterEnemySpawn();
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (Player.playerObject != null)
            {
                seeker.StartPath(rb.position, Player.playerObject.transform.position, OnPathComplete);
            }
        }
            
    }

    private void ShootAtPlayer()
    {
        if (Player.playerObject == null || Player.playerObject.GetComponent<Player>().isDead)
            return;

        if (canShoot)
        {
            List<Weapon> availableWeapons = new List<Weapon>(GetComponentsInChildren<Weapon>());
            if (availableWeapons.Any(w => w.IsOnCooldown))
                return;
            Weapon chosenWeapon = availableWeapons[Random.Range(0, availableWeapons.Count)];
            chosenWeapon.UseWeapon();
        }
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

        SpawnerManager.mainManager.RegisterEnemyDeath();
        SceneGoalManager.goalManager.RegisterEnemyDeath();
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
        if (Player.playerObject == null || Player.playerObject.GetComponent<Player>().isDead)
            return;

        gameObject.GetComponentInChildren<Animator>().SetBool("IsMoving", Mathf.Abs(rb.velocity.x) >= 0.1f);

        Vector3 playerPosition = Player.playerObject.transform.position;
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

        RaycastHit2D enemyLineOfSight =  Physics2D.Linecast(transform.position, playerPosition, 1 << LayerMask.GetMask(new string[] { "Ground", "Wall" }));

        if ((playerPosition - transform.position).magnitude >= playerShootDistance || (shouldRunAway && (playerPosition - transform.position).magnitude <= runAwayDistance))
        {
            if (path == null)
                return;
            else if (currentWaypoint >= path.vectorPath.Count)
            {
                return;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            if (shouldRunAway && (playerPosition - transform.position).magnitude <= runAwayDistance)
            {
                direction *= -1;
            }

            Vector2 force = new Vector2(direction.x * speed, rb.velocity.y);

            rb.velocity = force;

            if (direction.y != 1.0f)
            {
                Jumping jumpScript = gameObject.GetComponent<Jumping>();
                if (direction.y > 0.99f)
                {
                    jumpScript.TryJump();
                }
                else if (direction.y < -0.85f)
                {
                    StartCoroutine(jumpScript.TryJumpOffPlatform());
                }
            }

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            if(enemyLineOfSight.collider != null)
            {
                canShoot = false;
            }
            else
            {
                canShoot = true;
            }
            
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerShootDistance);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, runAwayDistance);
    }
}
