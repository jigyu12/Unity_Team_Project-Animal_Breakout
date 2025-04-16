using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : InGameManager
{
    [SerializeField]
    private int maxSkillCount = 4;

    public int MaxSkillCount=>maxSkillCount;

    private List<SkillPriorityItem> skills = new();
    private SkillQueue readySkillQueue = new SkillQueue();


    public float skillPerformInterval = 1f;
    private Coroutine coSkillPerform = null;

    private bool isBossStage;

    [SerializeField]
    private BossStatus skillTarget;

    public Action<List<SkillPriorityItem>> onSkillListUpdated;

    private void Awake()
    {
        BossManager.onSpawnBoss += OnSpawnBossHandler;
    }
    
 
    private void OnDestroy()
    {
        BossManager.onSpawnBoss -= OnSpawnBossHandler;
    }

    public override void Initialize()
    {
        base.Initialize();

        GameManager.StageManager.onBossStageEnter += () => isBossStage = true;
        BossStatus.onBossDead+= ()=>isBossStage = false;
    }


    public bool IsSkillExist(ISkill skill)
    {
        return IsSkillExist(skill.Id);
    }

    public bool IsSkillExist(int skillId)
    {
        return skills.Exists((target) => target.skill.Id == skillId);
    }
   

    public void AddSkill(int priority, ISkill skill)
    {
        var item = new SkillPriorityItem(priority, skill);
        skills.Add(item);
        onSkillListUpdated?.Invoke(skills);


        skill.InitializeSkilManager(this);
        skill.AddOnReadyAction(()=>readySkillQueue.Enqueue(item));
        skill.OnReady();
    }

    public float GetSkillInheritedForwardSpeed()
    {
        //절대 수정
        return GameManager.PlayerManager.playerRoot.GetComponent<MoveForward>().speed;
    }

    private void Update()
    {
        if(isBossStage)
        {
            BossStageUpdate();
        }
        else
        {
            NormalStageUpdate();
        }
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
            currentSkill.Perform(GameManager.PlayerManager.currentPlayerStatus.transform, skillTarget.transform, GameManager.PlayerManager.attackPower, skillTarget);
            yield return new WaitForSeconds(skillPerformInterval);
        }
        coSkillPerform = null;
    }

    public void WaitSkillCoolDownTime(ISkill skill)
    {
        StartCoroutine(CoWaitCoolTime(skill));
    }

    private IEnumerator CoWaitCoolTime(ISkill skill)
    {
        yield return new WaitForSeconds(skill.SkillData.coolDownTime);
        skill.OnReady();
    }

    private void OnSpawnBossHandler(BossStatus boss)
    {
        skillTarget = boss;
    }
}
