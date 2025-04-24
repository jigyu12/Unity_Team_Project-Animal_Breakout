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

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Run") || scene.name.Contains("InGame"))
        {
            StartCoroutine(LoadInGameResources());
        }
    }

    private IEnumerator LoadInGameResources()
    {
        // 로딩씬 Additive로 로드
        yield return SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

        // Addressables를 사용하여 비동기 로드 시작
        bool isLoadingComplete = false;
        LoadManager.Instance.LoadInGameResource(() =>
        {
            isLoadingComplete = true;
        });

        // 로딩 중 상태 표시
        while (!isLoadingComplete)
        {
            yield return null;
        }

        Debug.Log("Game Resources Loaded!");

        // 로딩 씬 언로드
        yield return SceneManager.UnloadSceneAsync("LoadingScene");

        // 로딩 완료 이벤트 호출
        onLoadComplete?.Invoke();
    }
}
