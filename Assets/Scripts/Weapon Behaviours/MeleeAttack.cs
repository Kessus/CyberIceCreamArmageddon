using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not used in game
public class MeleeAttack : WeaponBehaviour
{
    public Vector2 hitboxSize = new Vector2(1.0f, 1.0f);
    public float hitDelay = 0.0f;
    public int damageAmount = 40;
    public override void Execute() 
    {
        StartCoroutine(SwingWeapon());
    }

    private IEnumerator SwingWeapon()
    {
        yield return new WaitForSeconds(hitDelay);

        LayerMask layerMask = new LayerMask();
        if(gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            layerMask |= (1 << LayerMask.NameToLayer("EnemyProjectile"));
            layerMask |= (1 << LayerMask.NameToLayer("Enemy"));
        }
        else
        {
            layerMask |= (1 << LayerMask.NameToLayer("PlayerProjectile"));
            layerMask |= (1 << LayerMask.NameToLayer("Player"));
        }
        
        List<RaycastHit2D> swingHits = new List<RaycastHit2D>(Physics2D.BoxCastAll((Vector2)transform.position, hitboxSize, 0.0f, new Vector2(), 0.0f, layerMask));
        foreach (RaycastHit2D hit in swingHits)
        {
            Damage damageScript = hit.collider.gameObject.GetComponent<Damage>();
            Projectile projectileScript = hit.collider.gameObject.GetComponent<Projectile>();
            if (damageScript != null)
            {
                damageScript.ReceiveDamage(damageAmount);
                continue;
            }
            if (projectileScript != null)
            {
                Destroy(projectileScript.gameObject);
                continue;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, hitboxSize);
    }
}
