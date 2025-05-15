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
}
