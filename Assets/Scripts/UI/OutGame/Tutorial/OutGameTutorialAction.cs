using Excellcube.EasyTutorial;
using Excellcube.EasyTutorial.Page;
using Excellcube.EasyTutorial.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OutGameTutorialAction : MonoBehaviour
{
    private ECEasyTutorial outgameTutorial;
    [SerializeField]
    private OutGameManager outGameManager;
    private void Start()
    {
        outgameTutorial = gameObject.GetComponent<ECEasyTutorial>();
    }

    public void InitializeTutorialShopClickAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.AddListener(CompleteTutorialShopClickAction);
    }

    private void CompleteTutorialShopClickAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.RemoveListener(CompleteTutorialShopClickAction);

        TutorialEvent.Instance.Broadcast("Tutorial_ShopClick");
    }

    public void InitializeTutorialDoGachaAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        var button = pageData.HighlightTarget.GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(CompleteTutorialDoGachaAction);

    }

    private void CompleteTutorialDoGachaAction()
    {
        AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.DoSingleTutorialGacha).Invoke();

        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        var button = pageData.HighlightTarget.GetComponent<Button>();
        button.onClick.RemoveListener(CompleteTutorialShopClickAction);
        button.onClick.AddListener(button.GetComponent<GachaButton>().DoGacha);

       TutorialEvent.Instance.Broadcast("Tutorial_DoGacha");
    }

    public void InitializeTutorialDoGachaWaitAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        outGameManager.OutGameUIManager.onHideFullScreenPanel += CompleteTutorialDoGachaWaitAction;
    }

    private void CompleteTutorialDoGachaWaitAction()
    {
        outGameManager.OutGameUIManager.onHideFullScreenPanel -= CompleteTutorialDoGachaWaitAction;
        TutorialEvent.Instance.Broadcast("Tutorial_DoGachaWait");
    }

    [SerializeField]
    private Button lobbyButton;
    public void OnTutorialDoGachaWaitEndAction()
    {
        lobbyButton.onClick.Invoke();
    }


    public void InitializeTutorialAnimalClickAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.AddListener(CompleteTutorialAnimalClickAction);
    }

    private void CompleteTutorialAnimalClickAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.RemoveListener(CompleteTutorialAnimalClickAction);

        TutorialEvent.Instance.Broadcast("Tutorial_AnimalClick");
    }

    public void InitializeTutorialAnimalElementClickAction()
    {
        //가챠 머니 지급
        GameDataManager.Instance.GoldAnimalTokenKeySystem.AddGold(1000);
        GameDataManager.Instance.GoldAnimalTokenKeySystem.AddBronzeToken(10);

        StartCoroutine(SearchAnimalElementHighlightTargetCoroutine());
    }

    private IEnumerator SearchAnimalElementHighlightTargetCoroutine()
    {
        yield return null;
        
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.AddListener(CompleteTutorialAnimalElementClickAction);
    }

    private void CompleteTutorialAnimalElementClickAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.RemoveListener(CompleteTutorialAnimalElementClickAction);

        TutorialEvent.Instance.Broadcast("Tutorial_AnimalElementClick");
    }

    public void InitializeTutorialAnimalEnforceClickAction()
    {



        StartCoroutine(SearchAnimalEnforceHighlightTargetCoroutine());
    }
    
    private IEnumerator SearchAnimalEnforceHighlightTargetCoroutine()
    {
        yield return null;
        
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.AddListener(CompleteTutorialAnimalEnforcetClickAction);
    }

    private void CompleteTutorialAnimalEnforcetClickAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.RemoveListener(CompleteTutorialAnimalEnforcetClickAction);

        TutorialEvent.Instance.Broadcast("Tutorial_AnimalEnforceClick");
    }

    public void InitializeTutorialAnimalEnforceAlertClickAction()
    {
        StartCoroutine(SearchAnimalEnforceAlertHighlightTargetCoroutine());
    }
    
    private IEnumerator SearchAnimalEnforceAlertHighlightTargetCoroutine()
    {
        yield return null;
        
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.AddListener(CompleteTutorialAnimalEnforceAlertClickAction);
    }
    
    private void CompleteTutorialAnimalEnforceAlertClickAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.RemoveListener(CompleteTutorialAnimalEnforceAlertClickAction);

        TutorialEvent.Instance.Broadcast("Tutorial_AnimalEnforceAlertClick");
    }

    public void InitializeTutorialEndedAction()
    {
        //lobbyButton.onClick.Invoke();
        //튜토리얼 보상

        GameDataManager.Instance.GoldAnimalTokenKeySystem.AddGold(1000);
        GameDataManager.Instance.PlayerLevelSystem.AddExperienceValue(60);
    }
}