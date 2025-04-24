using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public abstract class UIRefresher : MonoBehaviour
{
    private RectTransform rectTransform;
    
    protected virtual void Awake()
    {
       TryGetComponent(out rectTransform);
    }

    protected virtual void StartRefresh()
    {
        Refresh();
    }

    protected virtual void StartRefreshAfterFrame()
    {
        StartCoroutine(RefreshPenalCoroutine());
    }
    
    protected virtual IEnumerator RefreshPenalCoroutine()
    {
        //yield return new WaitForEndOfFrame();
        yield return null;

        Refresh();
    }
    
    protected virtual void Refresh()
    {
        //rectTransform.gameObject.SetActive(false);
        //rectTransform.gameObject.SetActive(true);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        //Canvas.ForceUpdateCanvases();
    }
}