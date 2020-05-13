using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int damageAmount = 50;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage damageScript = collision.gameObject.GetComponent<Damage>();
        if (damageScript == null)
            return;

        damageScript.TakeDamage(damageAmount);
    }
}
