using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 8.0f;

    private Rigidbody2D rigidBody;
    private BoxCollider2D playerCollision;
    private bool isFacingLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        playerCollision = gameObject.GetComponent<BoxCollider2D>();
        isFacingLeft = transform.rotation.y != 0.0f;

        Assimilate();
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameUi.IsGamePaused)
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

        RotateTowardsMouseCursor();
    }

    private void FixedUpdate()
    {
        float movementInput = Input.GetAxis("Horizontal");

        if (movementInput != 0)
        { 
            rigidBody.velocity = new Vector2(movementSpeed * movementInput, rigidBody.velocity.y);
        }
        else
        {
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

    private void Assimilate()
    {
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
        }

        Enemy EnemyScript = gameObject.GetComponent<Enemy>();
        if (EnemyScript != null)
            EnemyScript.enabled = false;

        Camera.main.GetComponent<CameraFollow>().player = transform;

        GetComponent<Damage>().IsPlayer = true;

        GetComponent<Jumping>().jumpCooldown = 0.0f;

        RotateTowardsMouseCursor();
    }

    private void TryHijack()
    {
        float castDistance = 2.0f;
        RaycastHit2D hitResult = Physics2D.BoxCast(playerCollision.bounds.center, playerCollision.bounds.size, 0.0f, Vector2.right, castDistance, LayerMask.GetMask("Enemy"));
        if (hitResult.collider == null)
            return;
        GameObject hijackTarget = hitResult.collider.gameObject;

        if (hijackTarget.GetComponent<Enemy>().CanBeHijacked)
        {
            hijackTarget.GetComponent<Damage>().RegisterAssimilation(GetComponent<Damage>());

            hijackTarget.AddComponent<Player>();

            Destroy(gameObject);
        }
    }
}
