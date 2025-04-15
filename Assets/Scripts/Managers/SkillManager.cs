using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : InGameManager
{
    public enum SkillType
    {
        BossTarget,
        Utill,
    }

    [SerializeField]
    private int maxSkillCount = 4;

    public int MaxSkillCount=>maxSkillCount;

    private List<SkillPriorityItem> skills = new();
    private SkillQueue readySkillQueue = new SkillQueue();


    public float skillPerformInterval = 1f;
    private Coroutine coSkillPerform = null;
    [SerializeField]
    private GameObject skillTarget;

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
        enabled = false;
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
        //절대 수정 필!!!!!!!!!!!!!
        return GameManager.PlayerManager.playerRoot.GetComponent<MoveForward>().speed;
    }

    private void Update()
    {
        if (coSkillPerform == null && readySkillQueue.Count != 0)
        {
            coSkillPerform = StartCoroutine(CoroutinePerformSkill());
        }
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
        while (readySkillQueue.Count != 0)
        {
            var currentSkill = readySkillQueue.Dequeue();
            currentSkill.Perform(GameManager.PlayerManager.currentPlayerStatus.transform, skillTarget.transform);
            yield return new WaitForSeconds(skillPerformInterval);
        }
        coSkillPerform = null;
    }

    private void OnSpawnBossHandler(GameObject boss)
    {
        skillTarget = boss;
        enabled = true;

    }
}
