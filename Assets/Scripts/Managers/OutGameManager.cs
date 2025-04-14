using System.Collections.Generic;
using UnityEngine;

public class OutGameManager : MonoBehaviour
{
    #region manager
    
    private List<IManager> managers = new();
    
    private ObjectPoolManager objectPoolManager;
    public ObjectPoolManager ObjectPoolManager => objectPoolManager;
    
    private OutGameUIManager outGameUIManager;
    public OutGameUIManager OutGameUIManager => outGameUIManager;
    
    #endregion
    
    private void Start()
    {
        InitializeManagers();
    }
    
    private void OnDestroy()
    {
        foreach (var manager in managers)
        {
            manager.Clear();
        }
    }
    
    private void InitializeManagers()
    {
        objectPoolManager = new ObjectPoolManager();
        managers.Add(ObjectPoolManager);

        GameObject.FindGameObjectWithTag("OutGameUIManager").TryGetComponent(out outGameUIManager);
        outGameUIManager.SetOutGameManager(this);
        managers.Add(outGameUIManager);
        
        foreach (var manager in managers)
        {
            manager.Initialize();
        }
    }
    
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}