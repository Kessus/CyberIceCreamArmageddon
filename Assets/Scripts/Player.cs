using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 8.0f;

    public static GameObject playerObject;
    public bool isDead = false;
    public ParticleSystem hijackParticleSystem;
    public string hijackSoundName;

    private Rigidbody2D rigidBody;
    private BoxCollider2D playerCollision;
    private bool isFacingLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = gameObject;
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

        if (Input.GetKeyDown(KeyCode.T))
            GetComponent<Damage>().Die();

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
            WeaponCooldownIndicator.weaponCooldowns.RegisterWeaponChange(weapon);
        }

        Enemy EnemyScript = gameObject.GetComponent<Enemy>();
        if (EnemyScript != null)
            EnemyScript.enabled = false;

        playerObject = gameObject;

        GetComponent<Damage>().IsPlayer = true;
        GetComponent<Jumping>().jumpCooldown = 0.0f;
        RotateTowardsMouseCursor();
    }

    private void TryHijack()
    {
        RaycastHit2D hitResult = Physics2D.BoxCast(playerCollision.bounds.center, playerCollision.bounds.size, 0.0f, new Vector2(), 0.0f, LayerMask.GetMask("Enemy"));
        if (hitResult.collider == null)
            return;
        GameObject hijackTarget = hitResult.collider.gameObject;

        if (hijackTarget.GetComponent<Enemy>().CanBeHijacked)
        {
            if (hijackParticleSystem != null)
                hijackParticleSystem.Play();
            AudioManager.Manager.PlaySound(hijackSoundName);

            hijackTarget.GetComponent<Damage>().RegisterAssimilation(GetComponent<Damage>());
            hijackTarget.AddComponent<Player>();

            Destroy(gameObject);
        }
    }
}
