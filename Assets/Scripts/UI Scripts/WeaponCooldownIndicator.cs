using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCooldownIndicator : MonoBehaviour
{
    public Image leftWeaponIcon;
    public Image rightWeaponIcon;
    public Color cooldownTint;
    public static WeaponCooldownIndicator weaponCooldowns;

    public WeaponCooldownIndicator()
    {
        weaponCooldowns = this;
    }

    public void RegisterWeaponChange(Weapon newWeapon)
    {
        Image relevantIcon;
        if (newWeapon.triggerKey == "Fire1")
            relevantIcon = rightWeaponIcon;
        else
            relevantIcon = leftWeaponIcon;

        relevantIcon.sprite = newWeapon.weaponIcon;
    }

    public void ChangeWeaponCooldown(Weapon weapon, bool isOnCooldown)
    {
        Image relevantIcon;
        if (weapon.triggerKey == "Fire1")
            relevantIcon = rightWeaponIcon;
        else
            relevantIcon = leftWeaponIcon;

        if (isOnCooldown)
            relevantIcon.color = cooldownTint;
        else
            relevantIcon.color = Color.white;
    }
}
