using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class BaseCollisionBehaviour : ICollisionBehaviour
{
    public static Action<long> OnScoreChanged;
    protected long scoreToAdd;
    
    public void OnCollision(GameObject self, Collider other)
    {
        OnCollisionAction(self, other);
        //Object.Destroy(self);
        self.SetActive(false);
    }

    public void SetScoreToAdd(long scoreToAdd)
    {
        this.scoreToAdd = scoreToAdd;
    }
    
    protected abstract void OnCollisionAction(GameObject self, Collider other);
}