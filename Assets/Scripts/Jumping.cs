using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    public float jumpPower = 12.0f;
    public int jumpCount = 2;

    [SerializeField]
    private LayerMask groundCheckLayers = new LayerMask();

    private Rigidbody2D rigidBody;
    private BoxCollider2D playerCollision;
    private int remainingJumpCount = 2;
    private bool shouldCheckGroundContact = false;

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
        if (remainingJumpCount > 0)
        {
            remainingJumpCount--;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, (Vector2.up * jumpPower).y);
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
        float castDistance = 0.05f;
        RaycastHit2D downHitResult = Physics2D.BoxCast(playerCollision.bounds.center, playerCollision.bounds.size, 0.0f, Vector2.down, castDistance, groundCheckLayers);
        return downHitResult.collider != null;
    }
}
