using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : Singleton<SceneManagerEx>
{
    public Action onLoadComplete;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Run") || scene.name.Contains("InGame"))
        {
            StartCoroutine(LoadInGameResources());
        }
    }

    private IEnumerator LoadInGameResources()
    {
        // 로딩창 Additive로 띄우기
        yield return SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

        // 로딩 시작
        bool isLoadingComplete = false;
        LoadManager.Instance.LoadInGameResource(() =>
        {
            isLoadingComplete = true;
        });

        // 로딩 완료 대기
        while (!isLoadingComplete)
        {
            yield return null;
        }

        Debug.Log("Game Resources Loaded!");

        // 로딩 완료 후 로딩창 제거
        yield return SceneManager.UnloadSceneAsync("LoadingScene");

        // 로딩 완료
        onLoadComplete?.Invoke();
    }
}
