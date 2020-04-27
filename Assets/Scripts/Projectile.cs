using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    public int damageAmount = 100;
    [SerializeField] private LayerMask ignoredLayers = new LayerMask();
    public float timeToLive = 4.0f;
    public float speedVariation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * (speed + Random.Range(-speedVariation, speedVariation));
        gameObject.layer = LayerMask.NameToLayer(transform.parent.gameObject.layer == LayerMask.NameToLayer("Player") ? "PlayerProjectile" : "EnemyProjectile");
        Vector3 position = transform.position;
        transform.SetParent(null, false);
        transform.position = position;
        StartCoroutine(DespawnTimer());
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if((1 << hitInfo.gameObject.layer & ~ignoredLayers) != 0)
        {
            Damage damageScript  = hitInfo.GetComponent<Damage>();
            if (damageScript != null)
            {
                damageScript.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
