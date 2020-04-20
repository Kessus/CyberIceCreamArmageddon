using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponBehaviour weaponBehaviour;
    public string triggerKey = "Fire1";
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown(triggerKey))
        {
            weaponBehaviour.Execute();
        }
    }
}
