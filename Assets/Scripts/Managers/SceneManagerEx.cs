using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : Singleton<SceneManagerEx>, IManager
{
    public Action onReleaseScene;
    public Action onLoadScene;

    public void LoadScene(string sceneName)
    {
        onReleaseScene?.Invoke();

        SceneManager.LoadScene(sceneName);
        onLoadScene?.Invoke();
    }

    public void LoadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Initialize()
    {
        
    }

    public void Clear()
    {

    }
}
