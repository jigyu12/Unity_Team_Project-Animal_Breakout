using System;
using UnityEngine;

public class ConditionNode<T> : BehaviorNode<T> where T : MonoBehaviour
{
    protected Func<T, bool> condition;
    
    public ConditionNode(T context, Func<T, bool> condition) : base(context)
    {
        this.condition = condition;
    }

    protected override BTNodeState OnUpdate()
    {
        return condition(context) ? BTNodeState.Success : BTNodeState.Failure;
    }
}