using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPriorityComparer : Comparer<SkillPriorityItem>
{
    public override int Compare(SkillPriorityItem x, SkillPriorityItem y)
    {
        //레벨이 더 큰 Item이 먼저
        //동일 레벨끼리는 먼저 들어온 Item
        if (x.skill.Level != y.skill.Level)
        {
            return -(x.skill.Level - y.skill.Level);
        }
        else
        {
            return x.priority.CompareTo(y.priority);
        }
    }
}

