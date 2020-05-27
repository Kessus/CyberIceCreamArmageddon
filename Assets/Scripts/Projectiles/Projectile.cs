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
    public ParticleSystem hitParticle;
    public string shotSoundName;
    public string hitSoundName;
    

    [SerializeField]
    protected LayerMask ignoredLayers = new LayerMask();
    protected bool collisionLayersInitialized = false;
    protected bool hasDealtDamage = false;
    protected float remainingLifeTime;

    //Sets initial projectile velocity and its life timer
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

    //Unified method for creating projectiles that sets their data accordingly to the weapon that they were shot from
    public static GameObject CreateProjectile(GameObject projectile, GameObject weapon, Transform originPoint)
    {
        WeaponBehaviour behaviour = weapon.GetComponent<WeaponBehaviour>();

        GameObject createdProjectile = Instantiate(projectile, originPoint.position, originPoint.rotation);
        ChangeProjectileCollisionLayers(createdProjectile, weapon);

        return createdProjectile;
    }

    //Used to switch between player projectile and enemy projectile logic
    //Sets appropriate layers for collision detection
    private static void ChangeProjectileCollisionLayers(GameObject createdProjectile, GameObject instigator)
    {
        Projectile projectileScript = createdProjectile.GetComponent<Projectile>();
        if (instigator.layer == LayerMask.NameToLayer("Player"))
        {
            if (projectileScript.collisionLayersInitialized)
            {
                projectileScript.ignoredLayers &= ~LayerMask.GetMask(new string[] { "EnemyProjectile", "Enemy" });
            }
            createdProjectile.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
            projectileScript.ignoredLayers |= LayerMask.GetMask(new string[] { "PlayerProjectile", "Player" });
        }
        else
        {
            if (projectileScript.collisionLayersInitialized)
            {
                projectileScript.ignoredLayers &= ~LayerMask.GetMask(new string[] { "PlayerProjectile", "Player" });
            }
            createdProjectile.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
            projectileScript.ignoredLayers |= LayerMask.GetMask(new string[] { "EnemyProjectile", "Enemy" });
        }
        projectileScript.collisionLayersInitialized = true;
    }

    //Changes the projectile's properties and makes it fly towards its origin
    //It also refreshes its time to live
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

    //Logic for handling (or ignoring) objects hit by the projectile
    protected virtual void OnHit(Collider2D hitInfo)
    {
        //Projectiles don't react to dead enemies
        if (hitInfo.gameObject.GetComponent<Enemy>()?.isDead ?? false)
            return;

        //Check if the target's layer isn't ignored by the projectile and that the projectile hasn't already dealt damage to another enemy
        if ((1 << hitInfo.gameObject.layer & ~ignoredLayers) != 0 && !hasDealtDamage)
        {
            Damage damageScript = hitInfo.GetComponent<Damage>();
            BossDamage bossDamageScript = hitInfo.GetComponent<BossDamage>();
            if (damageScript != null)
                damageScript.ReceiveDamage(damageAmount);
            else if (bossDamageScript != null)
                bossDamageScript.ReceiveDamage(damageAmount);

            if (hitParticle != null)
            {
                Instantiate(hitParticle, transform.position, Quaternion.identity);
            }
            AudioManager.Manager.PlaySound(hitSoundName);

            hasDealtDamage = true;
            Destroy(gameObject);
        }
    }
}
