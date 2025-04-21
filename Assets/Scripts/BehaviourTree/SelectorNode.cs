using UnityEngine;

public class SelectorNode<T> : CompositeNode<T> where T : MonoBehaviour
{
    private int currentChild;

    public SelectorNode(T context) : base(context)
    {
    }

    public override void Reset()
    {
        base.Reset();
        
        currentChild = 0;
    }

    protected override void OnStart()
    {
        base.OnStart();
        
        currentChild = 0;
    }

    protected override BTNodeState OnUpdate()
    {
        while (currentChild < children.Count)
        {
            BTNodeState state = children[currentChild].Execute();
            
            if (state != BTNodeState.Failure)
            {
                return state;
            }
            
            ++currentChild;
        }
        
        return BTNodeState.Failure;
    }
}