public abstract class ItemBase : CollidableMapObject
{
    public abstract ItemType ItemType { get; protected set; }
}