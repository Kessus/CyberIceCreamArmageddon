using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Jumping : MonoBehaviour
{
    public float jumpPower = 12.0f;
    public int jumpCount = 2;
    public float jumpCooldown = 0.0f;

    [SerializeField]
    private LayerMask groundCheckLayers = new LayerMask();

    private Rigidbody2D rigidBody;
    private BoxCollider2D characterCollision;
    private int remainingJumpCount = 2;
    private bool shouldCheckGroundContact = false;
    private bool jumpOnCooldown = false;
    private bool isJumpingOff = false;

    // Start is called before the first frame update
    void Start()
    {
        characterCollision = gameObject.GetComponent<BoxCollider2D>();
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
            gameObject.GetComponent<Animator>().SetBool("IsInAir", true);
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
        if (platform.collider != null && platform.collider.gameObject.layer == LayerMask.NameToLayer("Platform") && !isJumpingOff)
        {
            gameObject.GetComponent<Animator>().SetBool("IsInAir", true);
            Physics2D.IgnoreCollision(characterCollision, platform.collider, true);
            isJumpingOff = true;
            yield return new WaitForSeconds(0.5f);
            Physics2D.IgnoreCollision(characterCollision, platform.collider, false);
            isJumpingOff = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collisionData)
    {
        shouldCheckGroundContact = true;
    }

    private void CheckGroundContact()
    {
        if (IsGrounded() && rigidBody.velocity.y < 0.1f && rigidBody.velocity.y > -0.1f)
        {
            gameObject.GetComponent<Animator>().SetBool("IsInAir", false);
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
        List<RaycastHit2D> collidingTargets = new List<RaycastHit2D>(Physics2D.BoxCastAll(characterCollision.bounds.min, new Vector2(characterCollision.bounds.size.x, characterCollision.bounds.size.y / 10), 0.0f, Vector2.down, castDistance, groundCheckLayers));
        return collidingTargets.FirstOrDefault(c => ((1 << c.collider.gameObject.layer) & groundCheckLayers) != 0);
    }
}
