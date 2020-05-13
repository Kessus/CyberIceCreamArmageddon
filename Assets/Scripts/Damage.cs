using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int maxBodyHealth = 100;
    public int maxGoggleHealth = 500;
    public float bodyFatigueDamagePercent = 0.1f;
    public float goggleFatigueDamagePercent = 0.15f;
    public float fatigueDelay = 6.0f;
    public float fatigueFrequency = 1.0f;
    public HealthBarBody healthBarScript;
    public HealthBarBody goggleHealthBarScript;

    [HideInInspector]
    public bool IsPlayer {
        get {
            return isPlayer;
        }
        set {
            if (!isPlayer && value)
            {
                InvokeRepeating("DealFatigueDamage", fatigueDelay, fatigueFrequency);
            }
            isPlayer = value;
        }
    }

    public int bodyHealth = 100;
    public int goggleHealth = 100;

    private bool isPlayer = false;

    private void Start()
    {
        bodyHealth = maxBodyHealth;
        goggleHealth = maxGoggleHealth;
        if (healthBarScript != null)
        {
            healthBarScript.SetMaxHealth(maxBodyHealth);
            healthBarScript.SetHealth(bodyHealth);
        }

        if(goggleHealthBarScript != null)
        {
            goggleHealthBarScript.SetMaxHealth(goggleHealth);
            goggleHealthBarScript.SetHealth(goggleHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isPlayer)
        {
            //For testing purposes
            damage *= 5;

            bodyHealth -= damage;
            if(healthBarScript != null)
                healthBarScript.SetHealth(Mathf.Clamp(bodyHealth, 0, maxBodyHealth));

            if (bodyHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            bodyHealth -= damage;
            int damageLeft = -bodyHealth;
            bodyHealth = Mathf.Clamp(bodyHealth, 0, maxBodyHealth);
            healthBarScript.SetHealth(bodyHealth);

            if(damageLeft > 0)
            {
                goggleHealth -= damageLeft;
                goggleHealthBarScript.SetHealth(Mathf.Clamp(goggleHealth, 0, maxGoggleHealth));
                if (goggleHealth <= 0)
                    Die();
            }
        }
    }

    public void RegisterAssimilation(Damage damageScript)
    {
        bodyHealth = maxBodyHealth;
        goggleHealth = damageScript.goggleHealth;
        healthBarScript = damageScript.healthBarScript;
        goggleHealthBarScript = damageScript.goggleHealthBarScript;
        healthBarScript.SetMaxHealth(maxBodyHealth);
        healthBarScript.SetHealth(bodyHealth);
    }

    private void DealFatigueDamage()
    {
        if(bodyHealth > 0)
            TakeDamage(Mathf.RoundToInt(bodyFatigueDamagePercent * maxBodyHealth));
        else
            TakeDamage(Mathf.RoundToInt(goggleFatigueDamagePercent * maxGoggleHealth));
    }

    void Die()
    {
        if (!isPlayer)
            Destroy(gameObject);
        else
        {
            Destroy(gameObject);
            Debug.Log("Player died!");
        }
    }
}
