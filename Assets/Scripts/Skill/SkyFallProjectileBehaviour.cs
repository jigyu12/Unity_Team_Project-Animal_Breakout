using UnityEngine;

public class SkyFallProjectileBehaviour : ProjectileBehaviour
{
    //public Vector3 skyOffset;

    private float firedTime;

    public override void Fire(Transform attacker, Transform target, float speed)
    {
        isArrival = false;
        this.target = target.position;
        this.speed = speed;

        transform.position = target.position;
        transform.rotation = Quaternion.LookRotation(attacker.forward);
        gameObject.SetActive(true);

        firedTime = Time.time;
    }

    private void Update()
    {
        //if (Time.time - firedTime > 5f / speed)
        if (Time.time - firedTime > 0.7f)
        {
            OnArrival();
            return;
        }
    }
}
