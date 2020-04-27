using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : WeaponBehaviour
{
    public GameObject projectileType;
    public GameObject firePoint;

    public override void Execute(bool firedByPlayer)
    {
        GameObject projectileInstance = Instantiate(projectileType, firePoint.transform.position, firePoint.transform.rotation, firePoint.transform);
    }
}
