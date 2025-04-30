using UnityEngine;

public class SkyFallProjectileBehaviour : ProjectileBehaviour
{
    public Vector3 skyOffset;

    private float firedTime;

    public override void Fire(Transform attacker, Transform target, float speed)
    {
        this.target = target.position;
        this.speed = speed;

        transform.position = target.position + skyOffset;
        gameObject.SetActive(true);

        firedTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - firedTime > 5f / speed)
        {
            OnArrival();
            return;
        }
    }
}
