using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTest : MonoBehaviour
{
    public Transform fireTarget;
    public Transform fireTransform;

    public int power;
    private AttackPowerStatus attacker;
    public DamageableStatus damager;

    private SkillFactory skillFactory;
    public List<AttackSkillData> attackSkillDatas = new();
    private List<AttackSkill> attackSkillList = new();

    [SerializeField]
    private SkillManager skillManager;

    public AttackSkill CurrentAttackSkill
    {
        get => attackSkillList[index];
    }
    private int index = 0;

    // Start is called before the first frame update
    private void Start()
    {
        skillManager.SetInititySkillMode();
        skillFactory = new SkillFactory();
        foreach (var data in attackSkillDatas)
        {
            attackSkillList.Add(skillFactory.CreateAttackSkill(data));
        }

        foreach(var skill in attackSkillList)
        {
            skill.InitializeSkilManager(skillManager);
        }

        attacker = GetComponent<AttackPowerStatus>();
        attacker.InitializeValue(power);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    private void Fire(int index)
    {
        StartCoroutine(CurrentAttackSkill.coPerform(attacker, damager, fireTransform, fireTarget));
    }

    [ContextMenu("Fire")]
    private void Fire()
    {
        Debug.Log("CurrentSkill : " + CurrentAttackSkill.SkillGroup + " Fire!");
        Fire(index);
        index++;
        index %= attackSkillList.Count;
        Debug.Log("CurrentSkill : " + CurrentAttackSkill.SkillGroup);
    }

}
