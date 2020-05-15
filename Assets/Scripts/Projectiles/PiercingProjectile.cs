using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingProjectile : Projectile
{
    public int pierceCount = 3;

    protected override void OnHit(Collider2D hitInfo)
    {
        if ((1 << hitInfo.gameObject.layer & ~ignoredLayers) != 0 && !hasDealtDamage)
        {
            Damage damageScript = hitInfo.gameObject.GetComponent<Damage>();
            if (damageScript != null)
            {
                    damageScript.TakeDamage(damageAmount);
            }
            pierceCount--;
            if(pierceCount <= 0)
            {
                hasDealtDamage = true;
                Destroy(gameObject);
            }

            if( hitInfo.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
}
