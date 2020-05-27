using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Destroys the corresponding particle when it finishes playing
public class ParticleAutoDestroy : MonoBehaviour
{
    private ParticleSystem destroyedParticleSystem;

    void Start()
    {
        destroyedParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!destroyedParticleSystem?.IsAlive() ?? false)
        {
            Destroy(gameObject);
        }
    }
}
