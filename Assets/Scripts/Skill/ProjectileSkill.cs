using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSkill : MonoBehaviour, ISkill
{
    private ProjectileSkillData skillData;

    [SerializeField]
    private float coolDownTime;
    [SerializeField]
    private float lastPerformedTime;
    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject projectilePrefab;

    public int Level
    {
        get; private set;
    } = 1;

    public bool IsReady
    {
        get => (Time.time >= lastPerformedTime + coolDownTime);
    }
    public float CoolTimeRatio
    {
        get => Mathf.Clamp01((Time.time - lastPerformedTime) / coolDownTime);
    }

    public int Id
    {
        get;
        private set;
    }

    private Action onReady;

    private SkillManager skillManager;

    public void InitializeSkilManager(SkillManager skillManager)
    {
        this.skillManager = skillManager;
    }

    public void Perform(Transform attackerTrs, Transform targetTrs, IAttacker attacker, IDamageable target)
    {
        //DoSomeThing;
        var projectile = Instantiate(projectilePrefab.gameObject).GetComponent<ProjectileBehaviour>();
        projectile.InitializeSkilManager(skillManager);

        projectile.onArrival += () => ApplyDamage(attacker, target);
        projectile.Fire(attackerTrs, targetTrs, speed);

        lastPerformedTime = Time.time;
        StartCoroutine(CoWaitCoolTime());
    }

    public void ApplyDamage(IAttacker attacker, IDamageable target)
    {
        //임시
        target.OnDamage(attacker.AttackPower);
    }

    private void OnEnable()
    {

    }

    public void UpgradeLevel()
    {
        Level++;

        //?좎룞?쇿뜝?숈삕 clamp?좎뙥怨ㅼ삕
    }

    //?좎뙓琉꾩삕?닷뜝?숈삕 enabled=false?좎떦?곗삕 ?좎떕?몄삕?좎떎琉꾩삕 ?좎룞?쇿뜝?숈삕
    private IEnumerator CoWaitCoolTime()
    {
        yield return new WaitForSeconds(coolDownTime);
        OnReady();
    }

    public void AddOnReadyAction(Action onReady)
    {
        this.onReady += onReady;
    }

    public void OnReady()
    {
        onReady?.Invoke();
    }
}
