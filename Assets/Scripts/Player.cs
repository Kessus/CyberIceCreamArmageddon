using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Has player-specific logic
public class Player : MonoBehaviour
{
    public float movementSpeed = 8.0f;

    public static GameObject playerObject;
    public bool isDead = false;
    public ParticleSystem hijackParticleSystem;
    public string hijackSoundName;
    [HideInInspector]
    public int damageDealt = 0;
    [HideInInspector]
    public int damageReceived = 0;

    private Rigidbody2D rigidBody;
    private BoxCollider2D playerCollision;
    private bool isFacingLeft = false;

    void Start()
    {
        playerObject = gameObject;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        playerCollision = gameObject.GetComponent<BoxCollider2D>();
        isFacingLeft = transform.rotation.y != 0.0f;

        Assimilate();
    }

    void Update()
    {
        if (InGameUi.IsGamePaused || isDead)
            return;

        if (Input.GetButtonDown("Jump"))
        {
            Jumping jumpScript = gameObject.GetComponent<Jumping>();
            if (Input.GetAxis("Vertical") < -0.5f)
                StartCoroutine(jumpScript.TryJumpOffPlatform());
            else
                jumpScript.TryJump();
        }
            
        if (Input.GetButtonDown("Hijack"))
            TryHijack();

        if (Input.GetKeyDown(KeyCode.T))
            GetComponent<Damage>().Die();

        RotateTowardsMouseCursor();
    }

    //Handles animations and movement
    private void FixedUpdate()
    {
        if (isDead)
            return;

        float movementInput = Input.GetAxis("Horizontal");

        if (movementInput != 0)
        {
            if(Mathf.Abs(movementInput) >= 0.5f)
            {
                Animator animator = gameObject.GetComponent<Animator>();
                animator.SetBool("IsMoving", true);
                bool isMovingRight = movementInput > 0.0f;

                //XOR operation. Either the character moves right and is facing right or is moving left and is facing left
                bool isMovingForward = isMovingRight ^ gameObject.transform.localRotation.y != 0.0f;  

                animator.SetBool("IsMovingForward", isMovingForward);
            }
            rigidBody.velocity = new Vector2(movementSpeed * movementInput, rigidBody.velocity.y);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
            rigidBody.velocity = new Vector2(0.0f, rigidBody.velocity.y);
        }
    }

    private void RotateTowardsMouseCursor()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x > transform.position.x && isFacingLeft)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            isFacingLeft = false;
        }
        else if (mousePosition.x < transform.position.x && !isFacingLeft)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            isFacingLeft = true;
        }
    }

    //Sets up needed data for a new player character
    private void Assimilate()
    {
        //Changes the GameObjects' layers
        Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
        foreach(Transform child in children)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Player");
            child.tag = "Player";
        }

        Weapon[] ownedWeapons = gameObject.GetComponentsInChildren<Weapon>();
        foreach(Weapon weapon in ownedWeapons)
        {
            weapon.reactToButtons = true;
            WeaponCooldownIndicator.weaponCooldowns.RegisterWeaponChange(weapon);
        }

        //Disables the enemy-controlling script
        Enemy EnemyScript = gameObject.GetComponent<Enemy>();
        if (EnemyScript != null)
            EnemyScript.enabled = false;

        playerObject = gameObject;

        GetComponent<Damage>().IsPlayer = true;
        GetComponent<Jumping>().jumpCooldown = 0.0f;
        RotateTowardsMouseCursor();
    }

    //Tries to transform any nearby enemy into a player character
    private void TryHijack()
    {
        List<RaycastHit2D> hitTargets = new List<RaycastHit2D>(Physics2D.BoxCastAll(playerCollision.bounds.center, playerCollision.bounds.size, 0.0f, new Vector2(), 0.0f, LayerMask.GetMask("Enemy")));
        RaycastHit2D hitResult = hitTargets.FirstOrDefault(c => (!c.collider.gameObject.GetComponent<Enemy>()?.isDead) ?? false);
        //RaycastHits are filtered by the target having an Enemy script and not being marked as dead
        if (!hitResult || hitResult.collider == null)
            return;
        GameObject hijackTarget = hitResult.collider.gameObject;

        if (hijackTarget.GetComponent<Enemy>().CanBeHijacked)
        {
            if (hijackParticleSystem != null)
                Instantiate(hijackParticleSystem, transform.position + (hijackTarget.transform.position - transform.position)/2, Quaternion.identity);
            AudioManager.Manager.PlaySound(hijackSoundName);

            //Setting up all of the necessary components for a to-be player character
            hijackTarget.GetComponent<Damage>().RegisterAssimilation(GetComponent<Damage>());
            hijackTarget.GetComponentInChildren<FaceSwap>().SwapFace(GetComponentInChildren<FaceSwap>());
            Player playerScript = hijackTarget.AddComponent<Player>();
            playerScript.hijackParticleSystem = hijackParticleSystem;
            playerScript.hijackSoundName = hijackSoundName;

            Destroy(gameObject);
        }
    }
}
