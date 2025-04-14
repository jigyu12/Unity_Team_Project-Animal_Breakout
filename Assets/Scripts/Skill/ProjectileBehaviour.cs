using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    private Transform target;

    private SkillManager skillManager;

    public void InitializeSkilManager(SkillManager skillManager)
    {
        this.skillManager = skillManager;
    }

    public void Fire(Transform attacker, Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;
  

        transform.position = attacker.position;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * (speed + skillManager?.GetSkillInheritedForwardSpeed() ?? 0f) * Time.deltaTime;
        transform.LookAt(this.target);
    }
}
