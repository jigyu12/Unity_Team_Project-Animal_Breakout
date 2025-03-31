using UnityEngine;

public class Wall : CollidableMapObject
{
    public override ObjectType ObjectType { get; protected set; }
    protected override ICollisionBehaviour CollisionBehaviour { get; set; }
    
    public WallType WallType { get; private set; } = WallType.None;

    public void Init(WallType wallType)
    {
        if (wallType == WallType.None || wallType == WallType.Count)
        {
            Debug.Assert(false, "Invalid WallType");
            return;
        }

        ObjectType = ObjectType.Wall;
        
        CollisionBehaviour = CollisionBehaviourFactory.GetWallBehaviour(wallType);
        
        WallType = wallType;
        
        // ToDo : Load Asset
        TryGetComponent(out MeshRenderer meshRenderer);
        switch (wallType)
        {
            case WallType.NormalWall:
                meshRenderer.material.color = Color.white;
                break;
            case WallType.ReinforcedWall:
                meshRenderer.material.color = Color.gray;
                break;
        }
    }
}