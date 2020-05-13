using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GravityTrap : MonoBehaviour
{
    public Vector2 effectOffset = new Vector2(0.0f, 0.0f);
    public Vector2 effectSize = new Vector2(0.0f, 0.0f);
    public LayerMask affectedLayers = new LayerMask();
    public float effectDuration = 3.0f;
    public float effectCooldown = 3.0f;

    private List<Vector2> objectVelocityCache = new List<Vector2>();
    private bool isActive = false;

    private void Start()
    {
        InvokeRepeating("StartCycle", 0.0f, effectDuration + effectCooldown);
    }

    private void StartCycle()
    {
        StartCoroutine("ActivateTrap");
    }

    private IEnumerator ActivateTrap()
    {
        isActive = true;
        List<RaycastHit2D> collidersInRange = new List<RaycastHit2D>(Physics2D.BoxCastAll(transform.position + new Vector3(effectOffset.x, effectOffset.y), effectSize, 0.0f, new Vector2(), 0.0f, affectedLayers));
        /*caughtObjects.AddRange(collidersInRange.Select(c => c.collider.gameObject).ToList());
        objectPositionCache.AddRange((caughtObjects.Select(o => o.transform.position).ToList()));*/
        foreach(RaycastHit2D collision in collidersInRange)
        {
            Rigidbody2D rigidBody = collision.collider.gameObject.GetComponent<Rigidbody2D>();
            if (rigidBody == null)
                continue;
            objectVelocityCache.Add(rigidBody.velocity);
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        yield return new WaitForSeconds(effectDuration);

        foreach (RaycastHit2D collision in collidersInRange)
        {
            Rigidbody2D rigidBody = collision.collider.gameObject.GetComponent<Rigidbody2D>();
            if (rigidBody == null)
                continue;
               
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidBody.velocity = objectVelocityCache[collidersInRange.IndexOf(collision)];
        }
        objectVelocityCache.Clear();
        isActive = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isActive? Color.red : Color.white;
        Gizmos.DrawWireCube(transform.position + new Vector3(effectOffset.x, effectOffset.y), new Vector3(effectSize.x, effectSize.y));
    }
}
