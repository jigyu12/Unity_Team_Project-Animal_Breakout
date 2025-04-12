using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class SkillQueue
{
    //È×µæ ¼ø¼­, ½ºÅ³
    private List<SkillPriorityItem> list = new();
    public int Count => list.Count;

    private Comparer<SkillPriorityItem> comparer = new SkillPriorityComparer();
    public void Enqueue(SkillPriorityItem skillPriorityItem)
    {
        list.Add(skillPriorityItem);
        list.Sort(comparer);
    }

    public void Enqueue(int value, ISkill skill)
    {
        Enqueue(new SkillPriorityItem(value, skill));
    }

    public ISkill Dequeue()
    {
        list.Sort(comparer);
        var value = list[0].skill;
        list.RemoveAt(0);
        return value;
    }
    public void Remove(ISkill skill)
    {
        int index = list.FindIndex(x => x.skill.Id == skill.Id);
        list.RemoveAt(index);
    }
}

public struct SkillPriorityItem : IEquatable<SkillPriorityItem>
{
    public SkillPriorityItem(int priority, ISkill skill)
    {
        this.priority = priority;
        this.skill = skill;
    }

    public int priority;
    public ISkill skill;

    public bool Equals(SkillPriorityItem other)
    {
        return skill.Id == other.skill.Id;
    }


}