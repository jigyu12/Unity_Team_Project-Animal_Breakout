using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public Slider progressBar;
    public TMP_Text progressText;

    private void Start()
    {
        progressBar.value = 0;
        progressText.text = "Loading...";
    }

    public void UpdateLoadingProgress(float progress)
    {
        progressBar.value = progress;
        progressText.text = $"Loading... {progress * 100:F1}%";
    }
}
