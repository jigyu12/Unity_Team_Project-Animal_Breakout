using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : InGameManager
{
    [SerializeField]
    private int maxSkillCount = 4;

    private List<SkillPriorityItem> skills = new();
    private SkillQueue readySkillQueue = new SkillQueue();


    public float skillPerformInterval = 1f;
    private Coroutine coSkillPerform = null;
    private IDamagerable skillTarget;

    private void Awake()
    {

    }

    public void SetSkillTarget(IDamagerable target)
    {
        skillTarget = target;
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
        //skill.OnReady += () => AddToReadySkillQueue(item);
    }

    private void AddToReadySkillQueue(SkillPriorityItem skillPriorityItem)
    {
        readySkillQueue.Enqueue(skillPriorityItem);
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
            currentSkill.Perform(GameManager.PlayerManager.currentPlayerStatus, skillTarget);
            yield return new WaitForSeconds(skillPerformInterval);
        }
        coSkillPerform = null;
    }

}
