using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class SkillQueue
{
    private List<KeyValuePair<int, ISkill>> list = new();

    private Comparer<KeyValuePair<int, ISkill>> comparer = new SkillPriorityComparer();

    //public bool Contains(ISkill skill)
    //{

    //}

    public void Enqueue(int value, ISkill skill)
    {
        list.Add(new KeyValuePair<int, ISkill>(value, skill));
        list.Sort(comparer);
    }

    public ISkill Dequeue()
    {
        return list[0].Value;
    }
}
