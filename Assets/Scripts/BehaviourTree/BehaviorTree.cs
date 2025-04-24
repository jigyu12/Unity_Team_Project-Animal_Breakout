using UnityEngine;

public class BehaviorTree<T> where T : MonoBehaviour
{
    private BehaviorNode<T> rootNode;

    public void SetRoot(BehaviorNode<T> node)
    {
        this.rootNode = node;
    }

    public BTNodeState Update()
    {
        return rootNode?.Execute() ?? BTNodeState.Failure;
    }

    public void Reset()
    {
        rootNode?.Reset();
    }
}