using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponBehaviour weaponBehaviour;
    public string triggerKey = "Fire1";
    public bool isAutomatic = false;
    public float cooldownDuration = 0.25f;
    public float visibilityTime = 0.3f;
    public Sprite weaponIcon;

    [HideInInspector]
    public bool reactToButtons = false;

    private bool isOnCooldown = false;
    private int visibilityChangeCounter = 0;
    private SpriteRenderer sprite;
    private bool usedWeapon = false;
    private Animator animator;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (InGameUi.IsGamePaused)
            return;

        if (!usedWeapon || isAutomatic)
        {
            if (Input.GetButton(triggerKey) && !isOnCooldown && reactToButtons)
            {
                usedWeapon = true;
                UseWeapon();
            }
        }
        else
        {
            usedWeapon = Input.GetButton(triggerKey) || isOnCooldown;
        }
    }

    public void UseWeapon()
    {
        weaponBehaviour.Execute();
        StartCoroutine(HandleCooldown());
        StartCoroutine(HandleVisibility());
        HandleAnimations();
    }

    private IEnumerator HandleCooldown()
    {
        isOnCooldown = true;
        WeaponCooldownIndicator.weaponCooldowns.ChangeWeaponCooldown(this, true);
        yield return new WaitForSeconds(cooldownDuration);
        WeaponCooldownIndicator.weaponCooldowns.ChangeWeaponCooldown(this, false);
        isOnCooldown = false;
    }

    private IEnumerator HandleVisibility()
    {
        sprite.enabled = true;
        visibilityChangeCounter++;
        yield return new WaitForSeconds(visibilityTime);
        visibilityChangeCounter--;
        if(visibilityChangeCounter == 0)
            sprite.enabled = false;
    }

    private void HandleAnimations()
    {
        if (animator == null)
            return;

        animator.SetTrigger("WeaponUsed");
    }
}
