using UnityEngine;

public class TimerNode<T> : DecoratorNode<T> where T : MonoBehaviour
{
    private float delayTime;
    private float startTime;
    private bool waiting;

    public TimerNode(T context, float delayTime) : base(context)
    {
        this.delayTime = delayTime;
        
        waiting = false;
    }

    protected override void OnStart()
    {
        base.OnStart();

        waiting = false;
    }

    public override void Reset()
    {
        base.Reset();

        waiting = false;
    }

    protected override BTNodeState ProcessChild()
    {
        if (!waiting)
        {
            var state = child.Execute();
            
            if (state == BTNodeState.Running)
            {
                return BTNodeState.Running;
            }

            startTime = Time.time;
            waiting = true;

            return BTNodeState.Running;
        }

        if (Time.time - startTime >= delayTime)
        {
            return BTNodeState.Success;
        }

        return BTNodeState.Running;
    }
}