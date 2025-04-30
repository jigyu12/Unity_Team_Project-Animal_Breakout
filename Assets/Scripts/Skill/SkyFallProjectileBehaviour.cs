using UnityEngine;

public class SkyFallProjectileBehaviour : ProjectileBehaviour
{
    public Vector3 skyOffset;

    public override void Fire(Transform attacker, Transform target, float speed)
    {
        this.target = target.position;
        this.speed = speed;

        transform.position = target.position + skyOffset;
        gameObject.SetActive(true);
    }

    //private void Update()
    //{
    //    if ((target - transform.position).magnitude <= arrivalThreshold)
    //    {
    //        OnArrival();
    //        return;
    //    }

    //    direction = (target - transform.position).normalized;
    //    transform.position += direction * (Speed + skillManager?.GetSkillInheritedForwardSpeed() ?? 0f) * Time.deltaTime;
    //    transform.LookAt(this.target);
    //}
}
