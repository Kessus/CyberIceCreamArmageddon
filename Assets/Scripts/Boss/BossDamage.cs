using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not used in the game
public class BossDamage : MonoBehaviour
{
    public bool isDefending = false;
    public int maxHealth = 2000;
    public int shieldStrength;
    
    private int currentHealth;
    private int shieldAmount;
    private BossBehaviour bossBehaviour;

    private void Start()
    {
        bossBehaviour = gameObject.GetComponent<BossBehaviour>();
        currentHealth = maxHealth;
    }
    public void ReceiveDamage(int damageAmount)
    {
        if (bossBehaviour.IsPoweringUp)
            return;

        if(shieldAmount > 0)
        {
            shieldAmount = Mathf.Max((shieldAmount - damageAmount), 0);
            if (shieldAmount == 0)
                bossBehaviour.DeactivateShield();
        }
        else
        {
            currentHealth -= damageAmount;
            if (currentHealth <= maxHealth * 0.5f && !bossBehaviour.IsPoweredUp)
                bossBehaviour.PowerUp();
            if(currentHealth <= 0)
                Die();
        }
    }

    private void Die()
    {

    }
}
