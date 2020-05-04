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
    }

    public void TakeDamage(int damage)
    {
        if (!isPlayer)
        {
            bodyHealth -= damage;

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
            if(damageLeft > 0)
            {
                goggleHealth -= damageLeft;
                if (goggleHealth <= 0)
                    Die();
            }
        }
    }

    public void RegisterAssimilation(Damage damageScript)
    {
        goggleHealth = damageScript.goggleHealth;
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
