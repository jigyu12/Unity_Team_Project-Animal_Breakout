using System;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    protected GameObject projectile;

    [SerializeField]
    protected float arrivalThreshold = 1f; //도착했다 치는 거리

    [SerializeField]
    public Vector3 firePositionOffset;
    public SfxClipId sfxClipThorwId;
    public SfxClipId sfxClipHitId;
    protected float speed;
    private static float gloablSpeed = 5f;
    public float Speed
    {
        get => speed * gloablSpeed;
    }
    protected Vector3 direction;
    protected Vector3 target;

    protected SkillManager skillManager;

    protected bool isArrival;
    public Action onArrival; //도착후 실행할 함수
    public Action targetGone;


    public void InitializeSkilManager(SkillManager skillManager)
    {
        this.skillManager = skillManager;
    }

    public virtual void Fire(Transform attacker, Transform target, float speed)
    {
        isArrival = false;
        this.target = target.position;
        this.speed = speed;


        transform.position = attacker.position + firePositionOffset;
        gameObject.SetActive(true);
        SoundManager.Instance.PlaySfx(sfxClipThorwId);
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
        if (isArrival)
            return;

        isArrival = true;
        SoundManager.Instance.PlaySfx(sfxClipHitId);
        onArrival?.Invoke();
        projectile.SetActive(false);
        Destroy(gameObject, 2f);
    }

    public void OnTargetGone()
    {
        Destroy(gameObject);
    }
}
