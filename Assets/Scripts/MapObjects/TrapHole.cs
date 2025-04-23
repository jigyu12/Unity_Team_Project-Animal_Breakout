public class TrapHole : Trap
{
    public void Initialize()
    {
        ObjectType = ObjectType.TrapHole;
        TrapType = TrapType.Hole;
        
        CollisionBehaviour = CollisionBehaviourFactory.GetTrapBehaviour(TrapType);
    }
}