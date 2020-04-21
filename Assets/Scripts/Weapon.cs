using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponBehaviour weaponBehaviour;
    public string triggerKey = "Fire1";
    public float cooldownDuration = 0.25f;
    public float visibilityTime = 0.3f;
    [HideInInspector]
    public bool reactToButtons = false;

    private bool isOnCooldown = false;
    private int visibilityChangeCounter = 0;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetButton(triggerKey) && !isOnCooldown && reactToButtons)
        {
            UseWeapon();
        }
    }

    public void UseWeapon()
    {
        weaponBehaviour.Execute();
        StartCoroutine(HandleCooldown());
        StartCoroutine(HandleVisibility());
    }

    IEnumerator HandleCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }

    IEnumerator HandleVisibility()
    {
        sprite.enabled = true;
        visibilityChangeCounter++;
        yield return new WaitForSeconds(visibilityTime);
        visibilityChangeCounter--;
        if(visibilityChangeCounter == 0)
            sprite.enabled = false;
    }
}
