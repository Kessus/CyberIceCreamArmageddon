using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : WeaponBehaviour
{
    public override void Execute()
    {
        GameObject projectileInstance = Projectile.CreateProjectile(projectileType, gameObject);
    }
}
