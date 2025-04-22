using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class SkillManager : InGameManager
{
    [SerializeField]
    private int maxSkillCount = 4;

    public float GlobalCoolDownTimeRate
    {
        get;
        private set;
    }


    public int MaxSkillCount => maxSkillCount;

    private List<ISkill> skills = new();
    private SkillQueue readySkillQueue = new SkillQueue();


    public float skillPerformInterval = 1f;
    private Coroutine coSkillPerform = null;


    [SerializeField]
    private BossStatus skillTarget;


    public SkillSelectionSystem SkillSelectionSystem
    {
        get;
        private set;
    }

    public SkillFactory SkillFactory
    {
        get;
        private set;
    }

    private void Awake()
    {
        SkillSelectionSystem = new SkillSelectionSystem(this, skills);
        SkillFactory = new SkillFactory();

        GlobalCoolDownTimeRate = 0f;

        BossManager.onSpawnBoss += OnSpawnBossHandler;
        BossStatus.onBossDead += ResetSkillTarget;
    }


    private void OnDestroy()
    {
        BossManager.onSpawnBoss -= OnSpawnBossHandler;
        BossStatus.onBossDead -= ResetSkillTarget;
    }

    private void ResetSkillTarget()
    {
        skillTarget = null;
    }

    public override void Initialize()
    {
        base.Initialize();

        GameManager.PlayerManager.onPlayerDead += () => enabled = false;
        GameManager.PlayerManager.playerStatus.onAlive += () => enabled = true;

    }

    public void OnSkillSelection()
    {
        GameManager.UIManager.Pause();
    }

    public void AddSkillToReadyQueue(SkillPriorityItem skillPriorityItem)
    {
        readySkillQueue.Enqueue(skillPriorityItem);
    }

    public float GetSkillInheritedForwardSpeed()
    {
        var moveforward = GameManager.PlayerManager.moveForward;
        return moveforward.enabled ? moveforward.speed : 0f;
    }

    private void Update()
    {
        if (GameManager.StageManager.IsPlayerInBossStage)
        {
            BossStageUpdate();
        }
        else
        {
            NormalStageUpdate();
        }

        UpdateSkillsCoolDownTime();
    }

    private void BossStageUpdate()
    {
        if (coSkillPerform == null && readySkillQueue.Count != 0)
        {
            coSkillPerform = StartCoroutine(CoroutinePerformSkill());
        }
    }

    private void NormalStageUpdate()
    {

    }

    private void OnDisable()
    {
        if (coSkillPerform != null)
        {
            StopCoroutine(coSkillPerform);
            coSkillPerform = null;
        }
    }

    private IEnumerator CoroutinePerformSkill()
    {
        if (skillTarget.IsDestroyed())
        {
            yield break;
        }

        while (readySkillQueue.Count != 0)
        {
            var currentSkill = readySkillQueue.Dequeue();
            PerformSkill(currentSkill);
            yield return new WaitForSeconds(skillPerformInterval);
        }
        coSkillPerform = null;
    }

    public void UpdateSkillsCoolDownTime()
    {
        foreach (var skill in skills)
        {
            skill.Update();
        }
    }

    public void PerformSkill(ISkill skill)
    {
        skill.Perform(GameManager.PlayerManager.playerStatus.transform, skillTarget?.transform ?? null, GameManager.PlayerManager.playerAttack, skillTarget);
    }

    public void AddGlobalCoolDownRate(float rate)
    {
        GlobalCoolDownTimeRate += rate;
    }

    private void OnSpawnBossHandler(BossStatus boss)
    {
        skillTarget = boss;
    }

    public bool IsSkillTargetValid()
    {
        return skillTarget != null;
    }

}