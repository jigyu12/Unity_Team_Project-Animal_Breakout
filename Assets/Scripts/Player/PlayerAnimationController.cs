using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private PlayerEyeExpressionController eyeAnimator;

    public readonly int dieAnimationHash = Animator.StringToHash("Die");
    public readonly int idleAnimationHash = Animator.StringToHash("Idle");
    public readonly int jumpAnimationHash = Animator.StringToHash("Jump");
    public readonly int runAnimationHash = Animator.StringToHash("Run");

    private void Start()
    {
        eyeAnimator = gameObject.GetComponent<PlayerEyeExpressionController>();
    }

    public void SetRunAnimation()
    {
        animator.SetTrigger(runAnimationHash);

        eyeAnimator.SetEyeExpression(PlayerEyeState.Annoyed);
    }

    public void SetDieAnimation()
    {
        animator.ResetTrigger(runAnimationHash);
        animator.SetTrigger(dieAnimationHash);

        eyeAnimator.SetEyeExpression(PlayerEyeState.Dead);
    }

    public void SetIdleAnimation()
    {
        animator.ResetTrigger(runAnimationHash);
        animator.SetTrigger(idleAnimationHash);

        eyeAnimator.SetEyeExpression(PlayerEyeState.Annoyed);
    }
}
