using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "InGameAssets/ProjectileSkill Data")]

public class ProjectileSkillData : AttackSkillData
{
    public ProjectileBehaviour projectileBehaviourPrefab;
    public float damageRate;
    public float speed;
    public int attackCount;
    public int projectileCount;
    public int elementalEffectAttackIndex;

}
