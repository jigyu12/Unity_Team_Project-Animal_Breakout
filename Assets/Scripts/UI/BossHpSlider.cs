using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHpSlider : MonoBehaviour
{
    private Slider hpSlider;
    
    private void Awake()
    {
        TryGetComponent(out hpSlider);
        
        gameObject.SetActive(false);
        
        BossManager.onSpawnBoss += OnSpawnBossHandler;
        BossStatus.onBossDead += OnBossDeadHandler;
        BossStatus.onBossCurrentHpChanged += OnBossCurrentHpChangedHandler;
    }

    private void OnEnable()
    {
        hpSlider.value = 1;
    }

    private void OnDestroy()
    {
        BossManager.onSpawnBoss -= OnSpawnBossHandler;
        BossStatus.onBossDead -= OnBossDeadHandler;
        BossStatus.onBossCurrentHpChanged -= OnBossCurrentHpChangedHandler;
    }

    private void OnSpawnBossHandler(BossStatus boss)
    {
        gameObject.SetActive(true);
    }
    
    private void OnBossDeadHandler()
    {
        gameObject.SetActive(false);
    }
    
    private void OnBossCurrentHpChangedHandler(float currentHp, float maxHp)
    {
        hpSlider.value = currentHp / maxHp;
    }
}