using UnityEngine;

public abstract class DecoratorNode<T> : BehaviorNode<T> where T : MonoBehaviour
{
    protected BehaviorNode<T> child;

    protected DecoratorNode(T context) : base(context)
    {
    }

    public void SetChild(BehaviorNode<T> child)
    {
        this.child = child;
    }

    public override void Reset()
    {
        base.Reset();
        
        child?.Reset();
    }

    protected override BTNodeState OnUpdate()
    {
        if (child == null)
        {
            return BTNodeState.Failure;
        }
        
        return ProcessChild();
    }

    protected abstract BTNodeState ProcessChild();
}