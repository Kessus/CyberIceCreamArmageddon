using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Regular projectile behaviour but doesn't get destroyed after hitting one target
//Instead has a specified number of targets that it can hit before getting destroyed
public class PiercingProjectile : Projectile
{
    public int pierceCount = 3;

    protected override void OnHit(Collider2D hitInfo)
    {
        //Check if the target's layer isn't ignored by the projectile and that the projectile hasn't already dealt damage to another enemy
        if ((1 << hitInfo.gameObject.layer & ~ignoredLayers) != 0 && !hasDealtDamage)
        {
            Damage damageScript = hitInfo.gameObject.GetComponent<Damage>();
            if (damageScript != null)
            {
                    damageScript.ReceiveDamage(damageAmount);
            }
            pierceCount--;
            if(hitParticle != null)
            {
                Instantiate(hitParticle, transform.position, Quaternion.identity);
            }
            if (pierceCount <= 0)
            {
                hasDealtDamage = true;
                Destroy(gameObject);
            }

            //When hitting the ground, a piercing projectile gets instantly destroyed without travelling further
            if( hitInfo.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
}
