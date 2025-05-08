using UnityEngine;
using UnityEngine.UI;

public class BossHpSlider : MonoBehaviour
{
    private Slider hpSlider;
    // [SerializeField] private GameObject StartPointSlider;

    private void Awake()
    {
        TryGetComponent(out hpSlider);

        gameObject.SetActive(false);

        BossManager.onSpawnBoss += OnSpawnBossHandler;
        BossStatus.onBossDeathAnimationEnded += OnBossDeadHandler;
        BossStatus.onBossCurrentHpChanged += OnBossCurrentHpChangedHandler;
        GameUIManager.onShowGameOverPanel += OnShowGameOverPanelHandler;
    }

    private void OnEnable()
    {
        hpSlider.value = 1;
        // StartPointSlider.SetActive(true);
    }
    private void OnDestroy()
    {
        BossManager.onSpawnBoss -= OnSpawnBossHandler;
        BossStatus.onBossDeathAnimationEnded -= OnBossDeadHandler;
        BossStatus.onBossCurrentHpChanged -= OnBossCurrentHpChangedHandler;
        GameUIManager.onShowGameOverPanel -= OnShowGameOverPanelHandler;

    }

    private void OnSpawnBossHandler(BossStatus boss)
    {
        gameObject.SetActive(true);
    }

    private void OnShowGameOverPanelHandler()
    {
        gameObject.SetActive(false);
    }

    private void OnBossDeadHandler()
    {
        gameObject.SetActive(false);
    }

    private void OnBossCurrentHpChangedHandler(float currentHp, float maxHp)
    {
        hpSlider.value = currentHp / maxHp;
        if (hpSlider.value <= 0)
        {
            // StartPointSlider.SetActive(false);
        }
    }
}