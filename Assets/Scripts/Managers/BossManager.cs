using System;
using UnityEngine;

public class BossManager : InGameManager
{
    [SerializeField] private GameObject bossPrefab;

    [SerializeField] private GameObject parentGameObjectToSpawnBoss;
    public static readonly Vector3 spawnLocalPosition = new(0f, 0.25f, 10f);

    private GameManager_new gameManager;

    private float lastBossBaseHp;
    private int bossStageSetCount;

    public static event Action<BossStatus> onSpawnBoss;

    [SerializeField][ReadOnly] private float bossMaxHp;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager);

        GameManager.StageManager.onBossStageEnter += SpawnBoss;

        lastBossBaseHp = 300000f;
        bossStageSetCount = 0;
    }

    private void OnDestroy()
    {
        gameManager.StageManager.onBossStageEnter -= SpawnBoss;
    }

    private void SpawnBoss()
    {
        var boss = Instantiate(bossPrefab, parentGameObjectToSpawnBoss.transform);
        boss.transform.localPosition = spawnLocalPosition;
        boss.TryGetComponent(out BossStatus bossStatus);
        boss.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        ++bossStageSetCount;
        switch (bossStageSetCount)
        {
            case < 2:
                {
                    bossMaxHp = lastBossBaseHp * MathF.Pow(1.15f, bossStageSetCount - 1);
                }
                break;
            case 2:
                {
                    bossMaxHp = lastBossBaseHp * MathF.Pow(1.15f, bossStageSetCount - 1);
                    lastBossBaseHp = bossMaxHp;
                }
                break;
            case < 4:
                {
                    bossMaxHp = lastBossBaseHp * MathF.Pow(1.18f, bossStageSetCount - 2);
                }
                break;
            case 4:
                {
                    bossMaxHp = lastBossBaseHp * MathF.Pow(1.18f, bossStageSetCount - 2);
                    lastBossBaseHp = bossMaxHp;
                }
                break;
            case < Int32.MaxValue:
                {
                    bossMaxHp = lastBossBaseHp * MathF.Pow(1.22f, bossStageSetCount - 4);
                }
                break;
        }
        bossStatus.InitializeStatus(bossMaxHp);

        SoundManager.Instance.PlaySfx(SfxClipId.BossTimeAlert);

        boss.TryGetComponent(out BossBehaviourController bossBehaviourController);
        bossBehaviourController?.InitBehaviorTree(BossBehaviourTreeType.Boss1BehaviourTree);

        onSpawnBoss?.Invoke(bossStatus);
    }
}