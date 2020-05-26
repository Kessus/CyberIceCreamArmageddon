using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private void FixedUpdate()
    {
        if (isDead)
            return;

        float movementInput = Input.GetAxis("Horizontal");

        if (movementInput != 0)
        {
            gameObject.GetComponent<Animator>().SetBool("IsMoving", Mathf.Abs(movementInput) >= 0.5f);
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
        List<RaycastHit2D> hitTargets = new List<RaycastHit2D>(Physics2D.BoxCastAll(playerCollision.bounds.center, playerCollision.bounds.size, 0.0f, new Vector2(), 0.0f, LayerMask.GetMask("Enemy")));
        RaycastHit2D hitResult = hitTargets.FirstOrDefault(c => (!c.collider.gameObject.GetComponent<Enemy>()?.isDead) ?? false);
        if (!hitResult || hitResult.collider == null)
            return;
        GameObject hijackTarget = hitResult.collider.gameObject;

        if (hijackTarget.GetComponent<Enemy>().CanBeHijacked)
        {
            if (hijackParticleSystem != null)
                Instantiate(hijackParticleSystem, transform.position, Quaternion.identity);
            AudioManager.Manager.PlaySound(hijackSoundName);

            hijackTarget.GetComponent<Damage>().RegisterAssimilation(GetComponent<Damage>());
            hijackTarget.GetComponentInChildren<FaceSwap>().SwapFace(GetComponentInChildren<FaceSwap>());
            Player playerScript = hijackTarget.AddComponent<Player>();
            playerScript.hijackParticleSystem = hijackParticleSystem;

            Destroy(gameObject);
        }
    }
}
