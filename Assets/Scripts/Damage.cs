﻿using System.Collections;
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
    public float enemyReceivedDamageMultiplier = 5.0f;
    public HealthBarBody healthBarScript;
    public HealthBarBody goggleHealthBarScript;
    public ParticleSystem damageParticleSystem;
    public ParticleSystem deathParticleSystem;
    public string damageSoundName;
    public string deathSoundName;

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



    public void ReceiveDamage(int damage)
    {
        if (!isPlayer)
        {
            //Enemies receive more damage than players
            damage = Mathf.FloorToInt(damage * enemyReceivedDamageMultiplier);

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
        if (damageParticleSystem != null)
            damageParticleSystem.Play();
        AudioManager.Manager.PlaySound(damageSoundName);
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
            ReceiveDamage(Mathf.RoundToInt(bodyFatigueDamagePercent * maxBodyHealth));
        else
            ReceiveDamage(Mathf.RoundToInt(goggleFatigueDamagePercent * maxGoggleHealth));
    }

    public void Die()
    {
        if (!isPlayer)
            Destroy(gameObject);
        else
        {
            Player.playerObject.GetComponent<Player>().isDead = true;
            DeathScreen.deathScreen.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        if (deathParticleSystem != null)
            deathParticleSystem.Play();
        AudioManager.Manager.PlaySound(deathSoundName);
    }
}
