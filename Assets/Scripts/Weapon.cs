using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A base class for each weapon
//Handles basic weapon logic and carries its data
//A weapon's effect is determined by the associated WeaponBehaviour script
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

    //Makes sure that the weapon doesn't dissapear between shots
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

        //Instantly hide hands and weapons when an enemy dies
        if (!ownerDead && (GetComponentInParent<Enemy>()?.isDead ?? false))
        {
            foreach (SpriteRenderer sprite in sprites)
                sprite.enabled = false;
            ownerDead = true;
        }

        //Handles shooting logic
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

    //Puts the weapon on cooldown and triggers UI cooldown logic if used by a player character
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

    //Shows and hides associated sprites
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
