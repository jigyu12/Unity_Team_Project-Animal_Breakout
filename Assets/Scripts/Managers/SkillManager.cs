using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillManager : InGameManager
{
    [SerializeField]
    private List<int> maxSkillTypeCount = new();

    public float GlobalCoolDownTimeRate
    {
        get;
        private set;
    }


    public int MaxSkillCount
    {
        get;
        private set;
    }

    private List<ISkill> skills = new();
    public IReadOnlyList<ISkill> Skills
    {
        get => skills;
    }

    private SkillQueue readyAttackSkillQueue = new SkillQueue();


    public float skillPerformInterval = 1f;
    private Coroutine coSkillPerform = null;
    public GameManager_new gameManager;

    [SerializeField]
    private BossStatus skillTarget;

    public SkillFactory SkillFactory
    {
        get;
        private set;
    }

    public SkillSelectionSystem SkillSelectionSystem
    {
        get;
        private set;
    }


    private void Awake()
    {
        SkillFactory = new SkillFactory();  //스킬팩토리가 스킬셀렉션시스템보다 먼저 생성되야 함
        SkillSelectionSystem = new SkillSelectionSystem(this, skills);

        GlobalCoolDownTimeRate = 0f;
        BossManager.onSpawnBoss += OnSpawnBossHandler;
        BossStatus.onBossDead += ResetSkillTarget;

        foreach (int count in maxSkillTypeCount)
        {
            MaxSkillCount += count;
        }
    }

    private void OnEnable()
    {
        coSkillPerform = null;
    }

    private void OnDestroy()
    {
        BossManager.onSpawnBoss -= OnSpawnBossHandler;
        BossStatus.onBossDead -= ResetSkillTarget;
    }

    private void ResetSkillTarget()
    {
        skillTarget = null;

        if (coSkillPerform != null)
        {
            StopCoroutine(coSkillPerform);
            coSkillPerform = null;
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        GameManager.PlayerManager.onPlayerDead += () => enabled = false;
        GameManager.PlayerManager.playerStatus.onAlive += () => enabled = true;
        GameManager.PlayerManager.playerExperience.onLevelChange += SkillSelection;
        gameManager = GameManager;
    }

    public void AddSkillToReadyQueue(SkillPriorityItem skillPriorityItem)
    {
        readyAttackSkillQueue.Enqueue(skillPriorityItem);
    }

    public float GetSkillInheritedForwardSpeed()
    {
        if(GameManager==null)
        {
            return 0;
        }

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
        if (coSkillPerform == null && readyAttackSkillQueue.Count != 0)
        {
            coSkillPerform = StartCoroutine(CoroutinePerformAttackSkill());
        }
    }

    private void NormalStageUpdate()
    {

    }

    private IEnumerator CoroutinePerformAttackSkill()
    {
        while (readyAttackSkillQueue.Count != 0)
        {
            if (!IsSkillTargetValid())
            {
                yield break;
            }

            var currentSkill = readyAttackSkillQueue.Dequeue();
            //스킬 실행을 기다린다.
            yield return StartCoroutine(currentSkill.coPerform(GameManager.PlayerManager.playerAttack, skillTarget, GameManager.PlayerManager.playerStatus.transform, skillTarget?.transform ?? null));
            //스킬당 인터벌을 기다린다.
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
        skill.Perform(GameManager.PlayerManager.playerAttack, skillTarget, GameManager.PlayerManager.playerStatus.transform, skillTarget?.transform ?? null);
    }

    public void PerformAttackSkill(AttackSkill skill)
    {
        StartCoroutine(skill.coPerform(GameManager.PlayerManager.playerAttack, skillTarget, GameManager.PlayerManager.playerStatus.transform, skillTarget?.transform ?? null));
    }

    public void AddGlobalCoolDownRate(float rate)
    {
        GlobalCoolDownTimeRate += rate;
    }

    private void OnSpawnBossHandler(BossStatus boss)
    {
        skillTarget = boss;

        foreach (var effect in skillTarget.GetComponents<StatusEffect>())
        {
            effect.InitializeSkillManager(this);
        }
    }

    public bool IsSkillTargetValid()
    {
        return skillTarget != null|| isInititySkillMode;
    }

    public void SkillSelection(int currentLevel, int exp)
    {
        if (currentLevel == 1)
        {
            return;
        }

        GameManager.UIManager.ShowUIElement(UIElementEnums.SkillSelectionPanel);
        GameManager.SetGameState(GameManager_new.GameState.GameStop);
    }

    public int GetSkillTypeMaxCount(SkillType type)
    {
        return maxSkillTypeCount[(int)type];
    }

    private bool isInititySkillMode = false;
    public void SetInititySkillMode()
    {
        isInititySkillMode = true;
    }
}