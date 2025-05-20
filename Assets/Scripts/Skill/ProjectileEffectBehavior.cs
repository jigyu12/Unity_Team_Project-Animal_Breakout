using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffectBehavior : MonoBehaviour
{
    [SerializeField] 
    private ProjectileBehaviour projectileBehaviour;

    [SerializeField]
    private ParticleEffect hitEffect;

    private void Start()
    {
        projectileBehaviour.onArrival += OnArrival;
    }

    private void OnArrival()
    {
        hitEffect.gameObject.SetActive(true);
        hitEffect.Effect();
    }
   
}
