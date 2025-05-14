using Excellcube.EasyTutorial;
using Excellcube.EasyTutorial.Page;
using Excellcube.EasyTutorial.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutGameTutorialAction : MonoBehaviour
{
    private ECEasyTutorial outgameTutorial;
    private void Start()
    {
        outgameTutorial=gameObject.GetComponent<ECEasyTutorial>();
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
        //var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        //pageData.HighlightTarget.GetComponent<Button>().onClick.AddListener(CompleteTutorialDoGachaAction);

        AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.DoSingleTutorialGacha).Invoke();
    }

    private void CompleteTutorialDoGachaAction()
    {
        var pageData = outgameTutorial.GetCurrentTutorialPageData() as ActionTutorialPageData;
        pageData.HighlightTarget.GetComponent<Button>().onClick.RemoveListener(CompleteTutorialShopClickAction);

        TutorialEvent.Instance.Broadcast("Tutorial_DoGacha");
    }
}
