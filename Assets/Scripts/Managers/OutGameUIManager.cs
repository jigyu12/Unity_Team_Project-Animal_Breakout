using UnityEngine;

public class OutGameUIManager : MonoBehaviour
{
    private void OnGameStartButtonClicked()
    {
        SceneManagerEx.Instance.LoadScene("RunCopy");
    }
}