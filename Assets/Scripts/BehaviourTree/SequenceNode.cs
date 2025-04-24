using UnityEngine;

public class SequenceNode<T> : CompositeNode<T> where T : MonoBehaviour
{
    private int currentChild;

    public SequenceNode(T context) : base(context)
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
            
            if (state != BTNodeState.Success)
            {
                return state;
            }
            
            ++currentChild;
        }
        
        return BTNodeState.Success;
    }
}