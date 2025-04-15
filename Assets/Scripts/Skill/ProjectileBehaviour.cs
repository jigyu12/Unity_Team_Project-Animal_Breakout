using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    private float arrivalThreshold = 1f; //도착했다 치는 거리

    private float speed;
    private Vector3 direction;
    private Transform target;

    private SkillManager skillManager;

    public Action onArrival; //도착후 실행할 함수


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
        if((target.transform.position - transform.position).magnitude <= arrivalThreshold)
        {
            onArrival?.Invoke();
            Destroy(gameObject);
        }

        direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * (speed + skillManager?.GetSkillInheritedForwardSpeed() ?? 0f) * Time.deltaTime;
        transform.LookAt(this.target);
    }
    
}
