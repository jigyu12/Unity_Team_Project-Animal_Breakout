using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class ResultPanelUI : MonoBehaviour
{
    public GameObject GameResultPanel;
    public Button ReStartButton;
    public Button GoMainButton;

    void Start()
    {
        ReStartButton.onClick.RemoveAllListeners();
        ReStartButton.onClick.AddListener(OnReStartButtonClicked);

        GoMainButton.onClick.RemoveAllListeners();
        GoMainButton.onClick.AddListener(OnGoMainButtonClicked);
    }

    private void OnReStartButtonClicked()
    {
        SceneManagerEx.Instance.LoadCurrentScene();
        GameResultPanel.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnGoMainButtonClicked()
    {
        GameObject gameManagerObject = GameObject.Find("GmManager");
        GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
        gameManager.LoadScene("MainTitleSceneTestMinjae");
        GameResultPanel.SetActive(false);
    }
}
