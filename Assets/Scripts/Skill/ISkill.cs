

using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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

    public float CoolTimeRatio
    {
        get;
    }

    public void InitializeSkilManager(SkillManager skillManager);

    void Perform(Transform attackerTrs, Transform targetTrs, IAttacker attacker = null, IDamagerable target = null);
    public void ApplyDamage(IAttacker attacker, IDamagerable target);

    public void OnReady();
    public void AddOnReadyAction(Action onReady);
    public void UpgradeLevel();

}
