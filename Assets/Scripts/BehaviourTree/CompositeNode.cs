using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode<T> : BehaviorNode<T> where T : MonoBehaviour
{
    protected readonly List<BehaviorNode<T>> children = new();

    protected CompositeNode(T context) : base(context)
    {
    }

    public void AddChild(BehaviorNode<T> child)
    {
        children.Add(child);
    }

    public bool RemoveChild(BehaviorNode<T> child)
    {
        return children.Remove(child);
    }

    public override void Reset()
    {
        base.Reset();

        foreach (var node in children)
        {
            node.Reset();
        }
    }
}