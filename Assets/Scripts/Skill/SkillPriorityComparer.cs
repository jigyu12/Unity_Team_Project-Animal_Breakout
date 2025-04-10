using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPriorityComparer : Comparer<KeyValuePair<int, ISkill>>
{
    public override int Compare(KeyValuePair<int, ISkill> x, KeyValuePair<int, ISkill> y)
    {
        if (x.Value.Level == y.Value.Level)
        {
            return x.Key.CompareTo(y.Key);
        }
        else
        {
            return x.Value.Level - y.Value.Level;
        }

    }
}
