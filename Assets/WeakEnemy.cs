using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("WeakenEnemy");
    }

    private IEnumerator WeakenEnemy()
    {
        yield return new WaitForSeconds(1);
        Damage damageScript = gameObject.GetComponent<Damage>();
        damageScript.ReceiveDamage(damageScript.maxBodyHealth / 2, true);
        damageScript.canBeDamaged = false;
    }
}
