using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDeflection : WeaponBehaviour
{
    public Vector2 hitboxSize = new Vector2(1.0f, 1.0f);
    public float hitDelay = 0.0f;
    public SpriteRenderer deflectorGun;
    private SpriteRenderer deflectorField;
    public override void Execute()
    {
        StartCoroutine(SwingWeapon());
    }

    private void Start()
    {
        deflectorField = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (deflectorField.enabled)
        {
            if (!deflectorGun.enabled)
                deflectorGun.enabled = true;
        }
        else
        {
            if (deflectorGun.enabled)
                deflectorGun.enabled = false;
        }
    }

    private IEnumerator SwingWeapon()
    {
        yield return new WaitForSeconds(hitDelay);

        LayerMask layerMask;
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            layerMask = LayerMask.GetMask(new string[] { "EnemyProjectile" });
        }
        else
        {
            layerMask = LayerMask.GetMask(new string[] { "PlayerProjectile" });
        }

        List<RaycastHit2D> swingHits = new List<RaycastHit2D>(Physics2D.BoxCastAll((Vector2)transform.position, hitboxSize, 0.0f, new Vector2(), 0.0f, layerMask));
        foreach (RaycastHit2D hit in swingHits)
        {
            Projectile projectileScript = hit.collider.gameObject.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.DeflectProjectile(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, hitboxSize);
    }
}
