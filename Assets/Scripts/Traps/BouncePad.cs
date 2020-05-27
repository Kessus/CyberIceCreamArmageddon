using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Launches players upwards when they collide with it
public class BouncePad : MonoBehaviour
{
    public float bounceForce = 10.0f;
    public ParticleSystem bounceParticleSystem;
    public string bounceSoundName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //This trap only reacts to players
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        //If there is no RigidBody2D found, trap does nothing
        Rigidbody2D otherRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRigidBody == null)
            return;


        if (bounceParticleSystem != null)
            bounceParticleSystem.Play();
        AudioManager.Manager.PlaySound(bounceSoundName);
        Vector2 resultForce = transform.up * bounceForce;

        //Zero out the y velocity of the trap's target
        otherRigidBody.velocity = new Vector2(otherRigidBody.velocity.x, 0.0f);
        otherRigidBody.AddForce(resultForce, ForceMode2D.Impulse);
        otherRigidBody.gameObject.GetComponent<Animator>().SetBool("IsInAir", true);
    }
}
