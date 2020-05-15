using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticProjectile : Projectile
{
    public float explosionRadius = 3.0f;

    protected override void OnHit(Collider2D hitInfo)
    {
        if ((1 << hitInfo.gameObject.layer & ~ignoredLayers) != 0 && !hasDealtDamage)
        {
            List<RaycastHit2D> targets = new List<RaycastHit2D>(Physics2D.CircleCastAll(transform.position, explosionRadius, new Vector2()));
            foreach (RaycastHit2D target in targets)
            {
                
                Damage damageScript = target.collider.gameObject.GetComponent<Damage>();
                if (damageScript != null && CanDamageTarget(target))
                {
                    damageScript.TakeDamage(damageAmount);
                }
            }
            hasDealtDamage = true;
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private bool CanDamageTarget(RaycastHit2D target)
    {
        if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
            return target.collider.gameObject.layer != LayerMask.NameToLayer("Player");

        else if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
            return target.collider.gameObject.layer != LayerMask.NameToLayer("Enemy");
        return false;
    }
}
