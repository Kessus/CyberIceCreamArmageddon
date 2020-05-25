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
    public bool IsOnCooldown { get; private set; } = false;
    [HideInInspector]
    public bool reactToButtons = false;

    
    private int visibilityChangeCounter = 0;
    private List<SpriteRenderer> sprites;
    private bool usedWeapon = false;
    private Animator animator;
    private bool ownerDead = false;

    private void Start()
    {
        sprites = new List<SpriteRenderer>(transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>());
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (InGameUi.IsGamePaused)
            return;

        if (!ownerDead && (GetComponentInParent<Enemy>()?.isDead ?? false))
        {
            foreach (SpriteRenderer sprite in sprites)
                sprite.enabled = false;
            ownerDead = true;
        }

        if (!usedWeapon || isAutomatic)
        {
            if (Input.GetButton(triggerKey) && !IsOnCooldown && reactToButtons)
            {
                usedWeapon = true;
                UseWeapon();
            }
        }
        else
        {
            usedWeapon = Input.GetButton(triggerKey) || IsOnCooldown;
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
        IsOnCooldown = true;
        if(gameObject.layer == LayerMask.NameToLayer("Player"))
            WeaponCooldownIndicator.weaponCooldowns.ChangeWeaponCooldown(this, true);
        yield return new WaitForSeconds(cooldownDuration);
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
            WeaponCooldownIndicator.weaponCooldowns.ChangeWeaponCooldown(this, false);
        IsOnCooldown = false;
    }

    private IEnumerator HandleVisibility()
    {
        foreach (SpriteRenderer sprite in sprites)
            sprite.enabled = true;
        visibilityChangeCounter++;
        yield return new WaitForSeconds(visibilityTime);
        visibilityChangeCounter--;
        if(visibilityChangeCounter == 0)
            foreach (SpriteRenderer sprite in sprites)
                sprite.enabled = false;
    }

    private void HandleAnimations()
    {
        if (animator == null)
            return;

        animator.SetTrigger("WeaponUsed");
    }
}
