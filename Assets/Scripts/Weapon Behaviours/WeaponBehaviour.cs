using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehaviour : MonoBehaviour
{
    public GameObject projectileType;
    public GameObject firePoint;

    public abstract void Execute();
}
