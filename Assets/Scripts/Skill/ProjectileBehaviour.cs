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
    private Vector3 target;

    private SkillManager skillManager;

    public Action onArrival; //도착후 실행할 함수
    public Action targetGone;


    public void InitializeSkilManager(SkillManager skillManager)
    {
        this.skillManager = skillManager;
    }

    public void Fire(Transform attacker, Transform target, float speed)
    {
        this.target = target.position;
        this.speed = speed;


        transform.position = attacker.position;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if ((target - transform.position).magnitude <= arrivalThreshold)
        {
            OnArrival();
            return;
        }

        direction = (target - transform.position).normalized;
        transform.position += direction * (speed + skillManager?.GetSkillInheritedForwardSpeed() ?? 0f) * Time.deltaTime;
        transform.LookAt(this.target);
    }

    public void OnArrival()
    {
        onArrival?.Invoke();
        Destroy(gameObject);
    }
    
    public void OnTargetGone()
    {
        Destroy(gameObject);
    }
}
