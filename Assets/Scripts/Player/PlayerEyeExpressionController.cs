using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum PlayerEyeState
    {
        Annoyed,
        Dead,
        Excited,
    }

public class PlayerEyeExpressionController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public readonly int eyeAnimationHash = Animator.StringToHash("Eye");


    public void SetEyeExpression(PlayerEyeState eye)
    {
        animator.SetInteger(eyeAnimationHash, (int)eye);
    }

    public void SetEyeExpressionImmidiate(PlayerEyeState eye)
    {
        animator.Play("Eyes_" + eye.ToString(), 1, 0.5f);
    }
}
