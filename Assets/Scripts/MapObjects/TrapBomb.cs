using System.Collections;
using UnityEngine;

public class TrapBomb : Trap
{
    private bool onCollision;
    
    private Animator animator;
    
    private void Awake()
    {
        TryGetComponent(out animator);
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !onCollision)
        {
            onCollision = true;

            if (other.transform.position.y >= 1f)
            {
                animator.SetTrigger("JumpAttack");
                
                StartCoroutine(JumpAttackAnimationControl());
            }
            else
            {
                animator.SetTrigger("Attack");
            }
            
            base.OnTriggerEnter(other);
        }
    }

    private IEnumerator JumpAttackAnimationControl()
    {
        yield return null;
        
        while (true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.normalizedTime < 0.5f)
            {
                animator.speed = 4f;
            }
            else
            {
                animator.speed = 1f;
                
                yield break;
            }
            
            yield return null;
        }
    }
    
    public void Initialize()
    {
        ObjectType = ObjectType.TrapBomb;
        TrapType = TrapType.Bomb;
        
        CollisionBehaviour = CollisionBehaviourFactory.GetTrapBehaviour(TrapType);
        
        onCollision = false;
    }
}