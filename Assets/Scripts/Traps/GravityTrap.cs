using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GravityTrap : MonoBehaviour
{
    public Vector2 effectOffset = new Vector2(0.0f, 0.0f);
    public Vector2 effectSize = new Vector2(0.0f, 0.0f);
    public LayerMask affectedLayers = new LayerMask();
    public float effectDuration = 3.0f;
    public float effectCooldown = 3.0f;
    public ParticleSystem activationParticleSystem;
    public ParticleSystem deactivationParticleSystem;
    public string activationSoundName;
    public string deactivationSoundName;

    private List<Vector2> objectVelocityCache = new List<Vector2>();
    private bool isActive = false;

    private void Start()
    {
        InvokeRepeating("StartCycle", 0.0f, effectDuration + effectCooldown);
    }

    private void StartCycle()
    {
        StartCoroutine("ActivateTrap");
    }

    private IEnumerator ActivateTrap()
    {
        isActive = true;
        if (activationParticleSystem != null)
            activationParticleSystem.Play();
        AudioManager.Manager.PlaySound(activationSoundName);

        List<RaycastHit2D> collidersInRange = new List<RaycastHit2D>(Physics2D.BoxCastAll(transform.position + (Vector3)effectOffset, effectSize, 0.0f, new Vector2(), 0.0f, affectedLayers));

        foreach(RaycastHit2D collision in collidersInRange)
        {
            Rigidbody2D rigidBody = collision.collider.gameObject.GetComponent<Rigidbody2D>();
            if (rigidBody == null)
                continue;
            objectVelocityCache.Add(rigidBody.velocity);
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        yield return new WaitForSeconds(effectDuration);

        if (deactivationParticleSystem != null)
            deactivationParticleSystem.Play();
        AudioManager.Manager.PlaySound(deactivationSoundName);
        foreach (RaycastHit2D collision in collidersInRange)
        {
            Rigidbody2D rigidBody = collision.collider.gameObject.GetComponent<Rigidbody2D>();
            if (rigidBody == null)
                continue;
               
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidBody.velocity = objectVelocityCache[collidersInRange.IndexOf(collision)];
        }
        objectVelocityCache.Clear();
        isActive = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isActive? Color.red : Color.white;
        Gizmos.DrawWireCube(transform.position + transform.right * effectOffset.x + transform.up * effectOffset.y, (Vector3)effectSize);
    }
}
