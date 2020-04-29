using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    public float jumpPower = 12.0f;
    public int jumpCount = 2;
    public float jumpCooldown = 0.0f;

    [SerializeField]
    private LayerMask groundCheckLayers = new LayerMask();

    private Rigidbody2D rigidBody;
    private BoxCollider2D playerCollision;
    private int remainingJumpCount = 2;
    private bool shouldCheckGroundContact = false;
    private bool jumpOnCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        playerCollision = gameObject.GetComponent<BoxCollider2D>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldCheckGroundContact)
        {
            CheckGroundContact();
            shouldCheckGroundContact = false;
        }
    }

    public void TryJump()
    {
        if (remainingJumpCount > 0 && !jumpOnCooldown)
        {
            remainingJumpCount--;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, (Vector2.up * jumpPower).y);
            StartCoroutine(HandleJumpCooldown());
        }
    }

    private IEnumerator HandleJumpCooldown()
    {
        jumpOnCooldown = true;
        yield return new WaitForSeconds(jumpCooldown);
        jumpOnCooldown = false;
   }

    public IEnumerator TryJumpOffPlatform()
    {
        RaycastHit2D platform = TryGetGroundObject();
        if (platform.collider != null && platform.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), true);
            platform.collider.usedByEffector = false;
            yield return new WaitForSeconds(0.5f);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), false);
            platform.collider.usedByEffector = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D collisionData)
    {
        shouldCheckGroundContact = true;
    }

    private void CheckGroundContact()
    {
        if (IsGrounded() && rigidBody.velocity.y < 0.1f)
        {
            remainingJumpCount = jumpCount;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D downHitResult = TryGetGroundObject();
        return downHitResult.collider != null;
    }

    private RaycastHit2D TryGetGroundObject()
    {
        float castDistance = 0.05f;
        return Physics2D.BoxCast(playerCollision.bounds.center, playerCollision.bounds.size, 0.0f, Vector2.down, castDistance, groundCheckLayers);
    }
}
