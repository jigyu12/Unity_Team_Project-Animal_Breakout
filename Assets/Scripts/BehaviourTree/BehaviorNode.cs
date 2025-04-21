using UnityEngine;

public abstract class BehaviorNode<T> where T : MonoBehaviour
{
    protected readonly T context;
    private bool isStarted = false;

    protected BehaviorNode(T context)
    {
        this.context = context;
    }

    public virtual void Reset()
    {
        isStarted = false;
    }

    protected virtual void OnStart()
    {
    }

    protected abstract BTNodeState OnUpdate();

    protected virtual void OnEnd()
    {
    }

    public BTNodeState Execute()
    {
        if (!isStarted)
        {
            isStarted = true;
            OnStart();
        }
        
        BTNodeState state = OnUpdate();
        
        if (state != BTNodeState.Running)
        {
            OnEnd();
            isStarted = false;
        }
        
        return state;
    }
} 