using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : WeaponBehaviour
{
    public int projectileCount = 1;
    public GameObject projectileType;
    public GameObject firePoint;
    public float maxSpreadAngle = 0.0f;
    public float knockbackDistance = 0.0f;

    public override void Execute(bool firedByPlayer)
    {
        for(int i = 0; i < projectileCount; i++)
        {
            GameObject projectile = Instantiate(projectileType, firePoint.transform.position, firePoint.transform.rotation, firePoint.transform);
            projectile.transform.Rotate(new Vector3(0.0f, 0.0f, Random.Range(-maxSpreadAngle, maxSpreadAngle)));
        }
        GetComponentInParent<Rigidbody2D>().AddForce(-(firePoint.transform.right * knockbackDistance));
    }
}
