using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponBehaviour weaponBehaviour;
    public string triggerKey = "Fire1";
    public float cooldownDuration = 0.25f;
    public float visibilityTime = 0.3f;

    bool isOnCooldown = false;
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetButton(triggerKey) && !isOnCooldown)
        {
            weaponBehaviour.Execute();
            StartCoroutine(HandleCooldown());
            StartCoroutine(HandleVisibility());
        }
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
        yield return new WaitForSeconds(visibilityTime);
        sprite.enabled = false;
    }
}
