using System;
using UnityEngine;

public class ActionNode<T> : BehaviorNode<T> where T : MonoBehaviour
{
    protected Func<T, BTNodeState> action;
    
    public ActionNode(T context, Func<T, BTNodeState> action) : base(context)
    {
        this.action = action;
    }

    protected override BTNodeState OnUpdate()
    {
        return action(context);
    }
}