public abstract class Trap : CollidableMapObject
{
    public override ObjectType ObjectType { get; protected set; } = ObjectType.None;
    protected override BaseCollisionBehaviour CollisionBehaviour { get; set; }

    public TrapType TrapType { get; protected set; } = TrapType.None;
}