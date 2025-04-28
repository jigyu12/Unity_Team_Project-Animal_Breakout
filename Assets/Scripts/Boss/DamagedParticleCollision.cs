using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DamagedParticleCollision : MonoBehaviour
{
    public static event Action<int> onTakeDamage; 
    
    private ParticleSystem particleSystem;

    [SerializeField] private GameObject parent;

    private void Start()
    {
        TryGetComponent(out particleSystem);
        
        GameObject.FindGameObjectWithTag("PlayerParent").transform.GetChild(3).TryGetComponent(out CapsuleCollider capsuleCollider);
        particleSystem.trigger.AddCollider(capsuleCollider);
    }

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enterParticles = new();
        int numEnter = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, enterParticles);

        for (int i = 0; i < numEnter; i++)
        {
            foreach (var particle in enterParticles)
            {
                onTakeDamage?.Invoke(1);

                parent.TryGetComponent(out BossProjectile bossProjectile);
                bossProjectile.Release();
            }
        }
    }
}