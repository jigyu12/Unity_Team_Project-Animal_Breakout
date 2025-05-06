using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class GachaButton : MonoBehaviour
{
    protected OutGameUIManager outGameUIManager;
    protected Button gachaButton;

    [SerializeField] protected TMP_Text headerText;
    [SerializeField] protected TMP_Text countText;

    protected virtual void Start()
    {
        GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
        outGameUIManager = outGameManager.OutGameUIManager;

        TryGetComponent(out gachaButton);
        gachaButton.onClick.RemoveAllListeners();
        gachaButton.onClick.AddListener(DoGacha);
    }

    protected void SetGachaButtonText(string headerText, string countText)
    {
        this.headerText.text = headerText;
        this.countText.text = countText;
    }

    public abstract void DoGacha();
}