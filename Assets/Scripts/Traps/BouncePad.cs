using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 10.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        Rigidbody2D otherRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRigidBody == null)
            return;

        Vector2 resultForce = transform.up * bounceForce;
        otherRigidBody.velocity = new Vector2(otherRigidBody.velocity.x, 0.0f);
        otherRigidBody.AddForce(resultForce, ForceMode2D.Impulse);
    }
}
