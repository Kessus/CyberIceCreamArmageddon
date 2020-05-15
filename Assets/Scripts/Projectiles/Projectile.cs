using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    public int damageAmount = 100;
    public float timeToLive = 4.0f;
    public float speedVariation = 0.0f;

    [SerializeField]
    protected LayerMask ignoredLayers = new LayerMask();
    protected bool collisionLayersInitialized = false;
    protected bool hasDealtDamage = false;
    protected float remainingLifeTime;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * (speed + Random.Range(-speedVariation, speedVariation));
        remainingLifeTime = timeToLive;
    }

    private void Update()
    {
        remainingLifeTime -= Time.deltaTime;
        if (remainingLifeTime <= 0)
            Destroy(gameObject);
    }

    public static GameObject CreateProjectile(GameObject projectile, GameObject weapon, Transform originPoint)
    {
        WeaponBehaviour behaviour = weapon.GetComponent<WeaponBehaviour>();

        GameObject createdProjectile = Instantiate(projectile, originPoint.position, originPoint.rotation);
        ChangeProjectileCollisionLayers(createdProjectile, weapon);

        return createdProjectile;
    }

    private static void ChangeProjectileCollisionLayers(GameObject createdProjectile, GameObject instigator)
    {
        Projectile projectileScript = createdProjectile.GetComponent<Projectile>();
        if (instigator.layer == LayerMask.NameToLayer("Player"))
        {
            if (projectileScript.collisionLayersInitialized)
            {
                projectileScript.ignoredLayers &= LayerMask.GetMask(new string[] { "EnemyProjectile", "Enemy" });
            }
            createdProjectile.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
            projectileScript.ignoredLayers |= LayerMask.GetMask(new string[] { "PlayerProjectile", "Player" });
        }
        else
        {
            if (projectileScript.collisionLayersInitialized)
            {
                projectileScript.ignoredLayers &= LayerMask.GetMask(new string[] { "PlayerProjectile", "Player" });
            }
            createdProjectile.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
            projectileScript.ignoredLayers |= LayerMask.GetMask(new string[] { "EnemyProjectile", "Enemy" });
        }
        projectileScript.collisionLayersInitialized = true;
    }

    public void DeflectProjectile(GameObject instigator)
    {
        ChangeProjectileCollisionLayers(gameObject, instigator);
        gameObject.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f), Space.Self);
        rb.velocity = transform.right * (speed + Random.Range(-speedVariation, speedVariation));
        remainingLifeTime = timeToLive;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        OnHit(hitInfo);
    }

    protected virtual void OnHit(Collider2D hitInfo)
    {
        if ((1 << hitInfo.gameObject.layer & ~ignoredLayers) != 0 && !hasDealtDamage)
        {
            Damage damageScript = hitInfo.GetComponent<Damage>();
            if (damageScript != null)
            {
                damageScript.TakeDamage(damageAmount);
            }
            hasDealtDamage = true;
            Destroy(gameObject);
        }
    }
}
