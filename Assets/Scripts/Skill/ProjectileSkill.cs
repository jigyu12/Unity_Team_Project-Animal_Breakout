using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSkill : MonoBehaviour, ISkill
{
    private float coolTime;
    private float lastPerformedTime;


    private float speed;

    public int Level
    {
        get; private set;
    }

    public bool IsReady
    {
        get => (Time.time <= lastPerformedTime + coolTime);
    }



    public void Perform(IAttacker attacker, IDamagerable target)
    {


        lastPerformedTime = Time.time;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public void UpgradeLevel()
    {
        Level++;

        //추후 clamp추가
    }
}
