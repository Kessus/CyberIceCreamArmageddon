﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shoots multiple projectiles, each in a randomized angle within specified range
public class SpreadShot : WeaponBehaviour
{
    public GameObject projectileType;
    public Transform originPoint;
    public int projectileCount = 1;
    public float maxSpreadAngle = 0.0f;
    public float knockbackDistance = 0.0f;

    public override void Execute()
    {
        for(int i = 0; i < projectileCount; i++)
        {
            GameObject projectile = Projectile.CreateProjectile(projectileType, gameObject, originPoint);
            projectile.transform.Rotate(new Vector3(0.0f, 0.0f, Random.Range(-maxSpreadAngle, maxSpreadAngle)));
        }
        GetComponentInParent<Rigidbody2D>().AddForce(-(originPoint.right * knockbackDistance));
    }
}
