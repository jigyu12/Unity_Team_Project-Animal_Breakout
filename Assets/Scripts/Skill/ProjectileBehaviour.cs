using System;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    protected float arrivalThreshold = 1f; //도착했다 치는 거리

    protected float speed;
    private static float gloablSpeed = 5f;
    public float Speed
    {
        get => speed * gloablSpeed;
    }
    protected Vector3 direction;
    protected Vector3 target;

    protected SkillManager skillManager;

    public Action onArrival; //도착후 실행할 함수
    public Action targetGone;


    public void InitializeSkilManager(SkillManager skillManager)
    {
        this.skillManager = skillManager;
    }

    public virtual void Fire(Transform attacker, Transform target, float speed)
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
        transform.position += direction * (Speed + skillManager?.GetSkillInheritedForwardSpeed() ?? 0f) * Time.deltaTime;
        transform.LookAt(this.target);
    }

    public virtual void OnArrival()
    {
        onArrival?.Invoke();
        Destroy(gameObject, 2f);
    }

    public void OnTargetGone()
    {
        Destroy(gameObject);
    }
}
