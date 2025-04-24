using UnityEngine;

public class Wall : CollidableMapObject
{
    public override ObjectType ObjectType { get; protected set; }
    protected override BaseCollisionBehaviour CollisionBehaviour { get; set; }
    
    public WallType WallType { get; private set; } = WallType.None;
    
    private const long ScoreToAddNormalWall = 1000;
    private const long ScoreToAddReinforcedWall = 1000;

    public void Initialize(WallType wallType)
    {
        if (wallType == WallType.None || wallType == WallType.Count)
        {
            Debug.Assert(false, "Invalid WallType");
            return;
        }

        ObjectType = ObjectType.Wall;
        
        CollisionBehaviour = CollisionBehaviourFactory.GetWallBehaviour(wallType);
        
        WallType = wallType;
        
        // ToDo : Load Asset & Table
        TryGetComponent(out MeshRenderer meshRenderer);
        switch (wallType)
        {
            case WallType.NormalWall:
            {
                meshRenderer.material.color = Color.white;
                CollisionBehaviour.SetScoreToAdd(ScoreToAddNormalWall);
            }
                break;
            case WallType.ReinforcedWall:
            {
                meshRenderer.material.color = Color.gray;
                CollisionBehaviour.SetScoreToAdd(ScoreToAddReinforcedWall);
            }
                break;
        }
    }
}