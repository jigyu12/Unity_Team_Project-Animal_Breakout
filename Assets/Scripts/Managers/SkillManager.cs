using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : InGameManager
{
    [SerializeField]
    private int maxSkillCount = 4;

    private List<ISkill> skills = new();

    public float skillPerformInterval = 1f;
   // private SortedList<int, ISkill> readySkillsQueue = new(new SkillPriorityComparer());

    private Coroutine coSkillPerform = null;
    private IDamagerable skillTarget;

    private void Awake()
    {
        
    }

    public void SetSkillTarget(IDamagerable target)
    {
        skillTarget = target;
    }

    public void AddSkill(ISkill skill)
    {
        //readySkillsQueue.
    }


    //public void RemoveSkill(ISkill skill)
    //{
    //    skills.Remove(skill);
    //    readySkillsQueue.Enqueue(skill);
    //}

    //private void Update()
    //{
    //    if (readySkillsQueue.Count != 0 && coSkillPerform == null)
    //    {
    //        coSkillPerform = StartCoroutine(CoroutinePerformSkill());
    //    }
    //}

    //private IEnumerator CoroutinePerformSkill()
    //{
    //    while (readySkillsQueue.Count != 0)
    //    {
    //        var currentSkill = readySkillsQueue.Dequeue();
    //        currentSkill.Perform(GameManager.PlayerManager.currentPlayerStatus, skillTarget);
    //        yield return new WaitForSecondsRealtime(skillPerformInterval);
    //    }
    //    coSkillPerform = null;
    //}

}
