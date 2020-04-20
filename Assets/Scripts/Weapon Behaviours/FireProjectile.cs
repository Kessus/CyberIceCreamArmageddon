using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : WeaponBehaviour
{
    public GameObject projectile;
    public GameObject firePoint;

    public override void Execute()
    {
        Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
    }
}
