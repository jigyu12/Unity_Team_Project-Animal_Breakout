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
        gachaButton.onClick.AddListener(DoGacha);
    }
    
    protected virtual void OnDestroy()
    {
        gachaButton.onClick.RemoveAllListeners();
    }

    protected void SetGachaButtonText(string headerText, string countText)
    {
        this.headerText.text = headerText;
        this.countText.text = countText;
    }
    
    protected void SetGachaButtonCountText(string countText)
    {
        this.countText.text = countText;
    }

    public abstract void DoGacha();
    
    public (int lackKey, long lackGold) GetLackKeyAndGoldCount(int keyCountToNeed)
    {
        if (GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentKey >= keyCountToNeed)
        {
            Debug.Log("Have enough key");
            
            return (0, 0);
        }
        
        int currentKeyCount = keyCountToNeed - GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentKey;
        long currentGold = GameDataManager.keyPrice * currentKeyCount;
        
        return (currentKeyCount, currentGold);
    }

}