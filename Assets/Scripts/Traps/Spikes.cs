using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Deals damage and plays a sound effect when colliding with a character
public class Spikes : MonoBehaviour
{
    public int damageAmount = 50;
    public string impaleSoundName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage damageScript = collision.gameObject.GetComponent<Damage>();
        if (damageScript == null)
            return;
        AudioManager.Manager.PlaySound(impaleSoundName);

        damageScript.ReceiveDamage(damageAmount);
    }
}
