using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> particleSystems = new();

    public void Effect()
    {
        foreach (var p in particleSystems)
        {
            p.Play();
        }
    }
}
