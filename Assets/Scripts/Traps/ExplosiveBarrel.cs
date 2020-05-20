using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public int blastDamage = 100;
    public float blastRadius = 5.0f;
    public bool shouldDrawRadius = true;
    private void OnDestroy()
    {
        List<Collider2D> collidersInRadius = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, blastRadius));

        foreach (Collider2D collider in collidersInRadius)
        {
            Damage damageScript = collider.gameObject.GetComponent<Damage>();
            if (damageScript == null)
                continue;

            damageScript.ReceiveDamage(blastDamage);
        }
    }

    private void OnDrawGizmos()
    {
        if (shouldDrawRadius)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, blastRadius);
        }
    }
}
