using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 5.0f;

    private Rigidbody2D rigidBody;
    private BoxCollider2D playerCollision;
    private bool isFacingLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        playerCollision = gameObject.GetComponent<BoxCollider2D>();

        Assimilate();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void FixedUpdate()
    {
        float movementInput = Input.GetAxis("Horizontal");

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

        if (movementInput != 0)
        { 
            rigidBody.velocity = new Vector2(movementSpeed * movementInput, rigidBody.velocity.y);
        }
    }

    private void Assimilate()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.tag = "Player";

        Weapon[] ownedWeapons = gameObject.GetComponentsInChildren<Weapon>();
        foreach(Weapon weapon in ownedWeapons)
        {
            weapon.reactToButtons = true;
        }

        HandMovement[] handMovementScripts = gameObject.GetComponentsInChildren<HandMovement>();
        foreach (HandMovement movementScript in handMovementScripts)
        {
            movementScript.aimTowardsPlayer = false;
        }

        Enemy EnemyScript = gameObject.GetComponent<Enemy>();
        if (EnemyScript != null)
            Destroy(EnemyScript);

        Camera.main.GetComponent<CameraFollow>().player = transform;
    }

    private void TryHijack()
    {
        float castDistance = 2.0f;
        RaycastHit2D hitResult = Physics2D.BoxCast(playerCollision.bounds.center, playerCollision.bounds.size, 0.0f, Vector2.right, castDistance, LayerMask.GetMask("Enemy"));
        if (hitResult.collider == null)
            return;

        GameObject hijackTarget = hitResult.collider.gameObject;

        hijackTarget.AddComponent<Player>();
        Destroy(gameObject);
    }
}
