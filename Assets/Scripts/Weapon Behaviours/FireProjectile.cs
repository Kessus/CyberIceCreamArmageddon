using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : WeaponBehaviour
{
    public GameObject projectileType;
    public Transform originPoint;
    public override void Execute()
    {
        GameObject projectileInstance = Projectile.CreateProjectile(projectileType, gameObject, originPoint);
    }
}
