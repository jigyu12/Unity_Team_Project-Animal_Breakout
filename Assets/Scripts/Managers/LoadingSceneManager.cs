using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LoadingSceneManager : MonoBehaviour
{
    public Slider progressBar;
    public TMP_Text progressText;

    private void Start()
    {
        StartLoading("Run");
    }

    public void StartLoading(string sceneName)
    {
        gameObject.SetActive(true);
        StartCoroutine(LoadGameResources(sceneName));
    }

    private IEnumerator LoadGameResources(string sceneName)
    {
        progressText.text = "Preparing...";
        List<int> runnerIDs = GameDataManager.Instance.GetRunnerIDs();
        bool isLoadingComplete = false;

        LoadManager.Instance.PreloadCharacters(runnerIDs, () =>
        {
            isLoadingComplete = true;
        });

        while (!isLoadingComplete)
        {
            UpdateLoadingProgress(0.5f);
            yield return null;
        }

        yield return StartCoroutine(LoadGameScene(sceneName));
    }

    private IEnumerator LoadGameScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            UpdateLoadingProgress(progress);

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        if (SceneManager.GetSceneByName("MainTitleScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("MainTitleScene");
        }
        SceneManager.UnloadSceneAsync("LoadingScene");
    }



    private void UpdateLoadingProgress(float progress)
    {
        progressBar.value = progress;
        progressText.text = $"Loading... {progress * 100:F1}%";
    }
}
