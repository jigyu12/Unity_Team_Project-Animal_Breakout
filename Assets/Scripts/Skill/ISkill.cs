

using System;
using UnityEditor.Experimental.GraphView;

public interface ISkill 
{
    public int Id
    {
        get;
    }

    public int Level
    {
        get;
    }

    public bool IsReady
    {
        get;
    }

    public Action OnReady
    {
        get;
    }

    void Perform(IAttacker attacker, IDamagerable target);
    void UpgradeLevel();

}
