using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCooldownIndicator : MonoBehaviour
{
    public Image secondaryWeaponIcon;
    public Image primaryWeaponIcon;
    public Slider secondaryWeaponCooldownSlider;
    public Slider primaryWeaponCooldownSlider;
    public Color cooldownTintColor;
    public static WeaponCooldownIndicator weaponCooldowns;

    private float primaryWeaponCooldownTimeLeft = 0.0f;
    private float secondaryWeaponCooldownTimeLeft = 0.0f;

    public WeaponCooldownIndicator()
    {
        weaponCooldowns = this;
    }

    //Cooldowns reduced over time
    private void Update()
    {
        primaryWeaponCooldownTimeLeft = Mathf.Max(0, primaryWeaponCooldownTimeLeft - Time.deltaTime);
        primaryWeaponCooldownSlider.value = primaryWeaponCooldownTimeLeft;
        secondaryWeaponCooldownTimeLeft = Mathf.Max(0, secondaryWeaponCooldownTimeLeft - Time.deltaTime);
        secondaryWeaponCooldownSlider.value = secondaryWeaponCooldownTimeLeft;
    }

    public void RegisterWeaponChange(Weapon newWeapon)
    {
        Image relevantIcon;
        if (newWeapon.triggerKey == "Fire1")
        {
            relevantIcon = primaryWeaponIcon;
            primaryWeaponCooldownSlider.maxValue = newWeapon.cooldownDuration;
        }
        else
        {
            relevantIcon = secondaryWeaponIcon;
            secondaryWeaponCooldownSlider.maxValue = newWeapon.cooldownDuration;
        }

        relevantIcon.sprite = newWeapon.weaponIcon;
    }

    //Visual representation of player's weapons' cooldowns
    public void ChangeWeaponCooldown(Weapon weapon, bool isOnCooldown)
    {
        if (isOnCooldown)
        {
            if (weapon.triggerKey == "Fire1")
            {
                primaryWeaponCooldownTimeLeft = weapon.cooldownDuration;
                primaryWeaponIcon.color = cooldownTintColor;
            }
            else
            {
                secondaryWeaponCooldownTimeLeft = weapon.cooldownDuration;
                secondaryWeaponIcon.color = cooldownTintColor;
            }
        }
        else
        {
            if (weapon.triggerKey == "Fire1")
            {
                primaryWeaponCooldownTimeLeft = 0.0f;
                primaryWeaponIcon.color = Color.white;
            }
            else
            {
                secondaryWeaponCooldownTimeLeft = 0.0f;
                secondaryWeaponIcon.color = Color.white;
            }
        }

    }
}
