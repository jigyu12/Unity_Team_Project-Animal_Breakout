using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSkill : MonoBehaviour, ISkill
{
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

    public void Perform(Transform attackerTrs, Transform targetTrs, IAttacker attacker = null, IDamageable target = null)
    {
        //DoSomeThing;
        var projectile = Instantiate(projectilePrefab.gameObject).GetComponent<ProjectileBehaviour>();
        projectile.InitializeSkilManager(skillManager);

        projectile.Fire(attackerTrs, targetTrs, speed);

        lastPerformedTime = Time.time;
        StartCoroutine(CoWaitCoolTime());
    }

    public void ApplyDamage(IAttacker attacker, IDamageable target)
    {

    }

    private void OnEnable()
    {

    }

    public void UpgradeLevel()
    {
        Level++;

        //���� clamp�߰�
    }

    //�ڷ�ƾ�� enabled=false�϶� �ȵ��Ƿ� ����
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
