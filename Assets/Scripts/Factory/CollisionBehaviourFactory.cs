using System;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionBehaviourFactory
{
    private static readonly Dictionary<TrapType, Func<ICollisionBehaviour>> Behaviours =
        new()
        {
            { TrapType.Bomb, () => new BombCollisionBehaviour() },
            { TrapType.Hole, () => new HoleCollisionBehaviour() },
        };

    public static ICollisionBehaviour GetBehaviour(TrapType trapType)
    {
        if (Behaviours.TryGetValue(trapType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find behaviour in TrapType: {trapType}");
        
        return null;
    }
}