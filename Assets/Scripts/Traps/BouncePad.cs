using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 10.0f;
    public ParticleSystem bounceParticleSystem;
    public string bounceSoundName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        Rigidbody2D otherRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRigidBody == null)
            return;

        if (bounceParticleSystem != null)
            bounceParticleSystem.Play();
        AudioManager.Manager.PlaySound(bounceSoundName);
        Vector2 resultForce = transform.up * bounceForce;
        otherRigidBody.velocity = new Vector2(otherRigidBody.velocity.x, 0.0f);
        otherRigidBody.AddForce(resultForce, ForceMode2D.Impulse);
        otherRigidBody.gameObject.GetComponent<Animator>().SetBool("IsInAir", true);
    }
}
