public class BossPatternSelectorNode : SelectorNode<BossBehaviourController>
{
    public BossPatternSelectorNode(BossBehaviourController context) : base(context)
    {
    }

    protected override BTNodeState OnUpdate()
    {
        context.SetBossPatternSelectRandomValue();
        
        return base.OnUpdate();
    }
}