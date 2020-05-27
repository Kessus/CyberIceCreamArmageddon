using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Deals damage in an area around the point of impact
//Is also affected by gravity
public class BallisticProjectile : Projectile
{
    public float explosionRadius = 3.0f;

    protected override void OnHit(Collider2D hitInfo)
    {
        //Projectiles don't react to dead enemies
        if (hitInfo.gameObject.GetComponent<Enemy>()?.isDead ?? false)
            return;

        if ((1 << hitInfo.gameObject.layer & ~ignoredLayers) != 0 && !hasDealtDamage )
        {
            List<RaycastHit2D> targets = new List<RaycastHit2D>(Physics2D.CircleCastAll(transform.position, explosionRadius, new Vector2()));
            foreach (RaycastHit2D target in targets)
            {
                
                Damage damageScript = target.collider.gameObject.GetComponent<Damage>();
                if (damageScript != null && CanDamageTarget(target))
                {
                    damageScript.ReceiveDamage(damageAmount);
                }
            }
            hasDealtDamage = true;
            if(hitParticle != null)
            {
                Instantiate(hitParticle, transform.position, Quaternion.identity);
            }
            //Make sure the sound of a projectile being shot no longer plays
            AudioManager.Manager.StopSound(shotSoundName, shotSoundIndex);
            AudioManager.Manager.PlaySound(hitSoundName);

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    //Used for making sure that there is no self-inflicted damage from the characters' weapons
    private bool CanDamageTarget(RaycastHit2D target)
    {
        if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
            return target.collider.gameObject.layer != LayerMask.NameToLayer("Player");

        else if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
            return target.collider.gameObject.layer != LayerMask.NameToLayer("Enemy");
        return false;
    }
}
