using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillQueue
{
    //�׵� ����, ��ų
    private List<SkillPriorityItem> list = new();
    public int Count => list.Count;

    private Comparer<SkillPriorityItem> comparer = new SkillPriorityComparer();
    public void Enqueue(SkillPriorityItem skillPriorityItem)
    {
        list.Add(skillPriorityItem);
        list.Sort(comparer);
        foreach (var item in list)
        {
            Debug.Log($"스킬큐 순서->{item.skill.AttackSkillData.skillGroup}");
        }
    }

    public void Enqueue(int value, AttackSkill skill)
    {
        Enqueue(new SkillPriorityItem(value, skill));
    }

    public AttackSkill Dequeue()
    {
        list.Sort(comparer);
        var value = list[0].skill;
        list.RemoveAt(0);
        foreach (var item in list)
        {
            Debug.Log($"스킬큐 순서->{item.skill.AttackSkillData.skillGroup}");
        }
        return value;
    }

    public void Remove(AttackSkill skill)
    {
        int index = list.FindIndex(x => x.skill.Id == skill.Id);
        list.RemoveAt(index);
    }
}

public struct SkillPriorityItem : IEquatable<SkillPriorityItem>
{
    public SkillPriorityItem(int priority, AttackSkill skill)
    {
        this.priority = priority;
        this.skill = skill;
    }

    public int priority;
    public AttackSkill skill;

    public bool Equals(SkillPriorityItem other)
    {
        return skill.Id == other.skill.Id;
    }


}