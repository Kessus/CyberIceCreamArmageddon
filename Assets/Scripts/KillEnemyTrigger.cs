using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillEnemyTrigger : MonoBehaviour
{
    public GameObject enemyToKill;
    public Text enemyCountText;
    private bool killedEnemy = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !killedEnemy)
        {
            enemyToKill.GetComponent<Damage>().ReceiveDamage(1000);
            killedEnemy = true;
            enemyCountText.enabled = true;
        }
    }
}
