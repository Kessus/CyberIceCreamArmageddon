using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : WeaponBehaviour
{
    public int projectileCount = 1;
    public float maxSpreadAngle = 0.0f;
    public float knockbackDistance = 0.0f;

    public override void Execute()
    {
        for(int i = 0; i < projectileCount; i++)
        {
            GameObject projectile = Projectile.CreateProjectile(projectileType, gameObject);
            projectile.transform.Rotate(new Vector3(0.0f, 0.0f, Random.Range(-maxSpreadAngle, maxSpreadAngle)));
        }
        GetComponentInParent<Rigidbody2D>().AddForce(-(firePoint.transform.right * knockbackDistance));
    }
}
