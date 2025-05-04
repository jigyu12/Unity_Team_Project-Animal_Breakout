using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class GachaButton : MonoBehaviour
{
    protected OutGameUIManager outGameUIManager;
    protected Button gachaButton;

    protected void Start()
    {
        GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
        outGameUIManager = outGameManager.OutGameUIManager;

        TryGetComponent(out gachaButton);
        gachaButton.onClick.RemoveAllListeners();
        gachaButton.onClick.AddListener(DoGacha);
    }

    public abstract void DoGacha();
}