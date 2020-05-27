using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Upon being destroyed (by receiving damage) explodes dealing a large amount of damage in an area
public class ExplosiveBarrel : MonoBehaviour
{
    public int blastDamage = 100;
    public float blastRadius = 5.0f;
    public bool shouldDrawRadius = true;
    public string explosionSoundName;
    public ParticleSystem explosionParticleSystem;
    private void OnDestroy()
    {
        List<Collider2D> collidersInRadius = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, blastRadius));

        if (explosionParticleSystem != null)
            explosionParticleSystem.Play();
        AudioManager.Manager.PlaySound(explosionSoundName);
        foreach (Collider2D collider in collidersInRadius)
        {
            Damage damageScript = collider.gameObject.GetComponent<Damage>();
            if (damageScript == null)
                continue;

            damageScript.ReceiveDamage(blastDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (shouldDrawRadius)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, blastRadius);
        }
    }
}
