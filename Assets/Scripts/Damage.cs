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
    public float enemyReceivedDamageMultiplier = 5.0f;
    public HealthBarBody healthBarScript;
    public HealthBarBody goggleHealthBarScript;
    public ParticleSystem damageParticleSystem;
    public string damageSoundName;
    public string deathSoundName;
    public bool destroyInstantlyOnDeath = false;
    [HideInInspector]
    public bool canBeDamaged = true;

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

    public void ReceiveDamage(int damage, bool isFromFatigue = false)
    {
        if (Player.playerObject.GetComponent<Player>().isDead || !canBeDamaged)
            return;

        if (!isPlayer)
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            if (enemy != null && enemy.isDead)
                return;

            //Enemies receive more damage than players
            damage = Mathf.FloorToInt(damage * enemyReceivedDamageMultiplier);
            
            Player.playerObject.GetComponent<Player>().damageDealt += damage;

            bodyHealth -= damage;
            if(healthBarScript != null)
                healthBarScript.SetHealth(Mathf.Clamp(bodyHealth, 0, maxBodyHealth));

            if (bodyHealth <= 0)
            {
                if (destroyInstantlyOnDeath)
                    Destroy(gameObject);
                else
                {
                    Animator animator = gameObject.GetComponent<Animator>();
                    if (animator != null)
                        animator.SetTrigger("HasDied");
                    enemy.isDead = true;
                }
            }
        }
        else
        {
            Player.playerObject.GetComponent<Player>().damageReceived += damage;
            bodyHealth -= damage;
            int damageLeft = -bodyHealth;
            bodyHealth = Mathf.Clamp(bodyHealth, 0, maxBodyHealth);
            healthBarScript.SetHealth(bodyHealth);

            if(damageLeft > 0)
            {
                goggleHealth -= damageLeft;
                goggleHealthBarScript.SetHealth(Mathf.Clamp(goggleHealth, 0, maxGoggleHealth));
                if (goggleHealth <= 0)
                {
                    gameObject.GetComponent<Animator>().SetTrigger("HasDied");
                    Player.playerObject.GetComponent<Player>().isDead = true;
                }
            }
        }
        if (damageParticleSystem != null && !isFromFatigue)
            Instantiate(damageParticleSystem, transform.position, Quaternion.identity);
        AudioManager.Manager.PlaySound(damageSoundName);
    }

    public void RegisterAssimilation(Damage damageScript)
    {
        bodyHealth = maxBodyHealth;
        goggleHealth = damageScript.goggleHealth;
        maxGoggleHealth = damageScript.maxGoggleHealth;
        healthBarScript = damageScript.healthBarScript;
        goggleHealthBarScript = damageScript.goggleHealthBarScript;
        healthBarScript.SetMaxHealth(maxBodyHealth);
        healthBarScript.SetHealth(bodyHealth);
    }

    private void DealFatigueDamage()
    {
        if (SceneGoalManager.goalManager.StageComplete)
            return;

        if(bodyHealth > 0)
            ReceiveDamage(Mathf.RoundToInt(bodyFatigueDamagePercent * maxBodyHealth), true);
        else
            ReceiveDamage(Mathf.RoundToInt(goggleFatigueDamagePercent * maxGoggleHealth), true);
    }

    public void Die()
    {
        //if (deathParticleSystem != null)
        //    deathParticleSystem.Play();
        AudioManager.Manager.PlaySound(deathSoundName);

        if (isPlayer)
        {
            DeathScreen.deathScreen.gameObject.SetActive(true);
        }
        else
        {
            healthBarScript.gameObject.SetActive(false);
            GetComponent<Enemy>().enabled = false;
            StartCoroutine("DestroyBody");
        }
    }

    public IEnumerator DestroyBody()
    {
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }
}
