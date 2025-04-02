using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutGameUIManager : MonoBehaviour
{
    public TMP_Text totalCoinText;
    public TMP_Text maxScoreText;
    public Button gameStartButton;

    private void Start()
    {
        gameStartButton.onClick.RemoveAllListeners();
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);

        GameDataManager.Instance.Initialize();
    }

    public void SetTotalCoinText(long coin)
    {
        totalCoinText.text = $"Total Coins : {coin.ToString()}";
    }

    public void SetMaxScoreText(long score)
    {
        maxScoreText.text = $"Max Score : {score.ToString()}";
    }

    private void OnGameStartButtonClicked()
    {
        SceneManagerEx.Instance.LoadScene("RunCopyMinjae");
    }
}