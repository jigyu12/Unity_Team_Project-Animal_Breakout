using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutGroupController : MonoBehaviour
{
    private GridLayoutGroup gridLayoutGroup;
    private VerticalLayoutGroup verticalLayoutGroup;
    private HorizontalLayoutGroup horizontalLayoutGroup;

    private readonly List<LayoutElement> layoutElementList = new();

    private void Awake()
    {
        OutGameUIManager.onSwitchActiveLayoutGroupControllers += SwitchActiveLayoutGroupControllerHandler;
    }

    private void OnDestroy()
    {
        OutGameUIManager.onSwitchActiveLayoutGroupControllers -= SwitchActiveLayoutGroupControllerHandler;
    }

    private void Start()
    {
        TryGetComponent(out gridLayoutGroup);
        TryGetComponent(out verticalLayoutGroup);
        TryGetComponent(out horizontalLayoutGroup);
        
        InitializeLayoutElementList();
    }

    private void SwitchActiveLayoutGroupControllerHandler(bool isActive)
    {
        if (isActive)
        {
            EnableAllLayoutComponents();
        }
        else
        {
            DisableAllLayoutComponents();
        }
    }
    
    public void EnableAllLayoutComponents()
    {
        if (gridLayoutGroup is not null)
        {
            gridLayoutGroup.enabled = true;
        }

        if (verticalLayoutGroup is not null)
        {
            verticalLayoutGroup.enabled = true;
        }

        if (horizontalLayoutGroup is not null)
        {
            horizontalLayoutGroup.enabled = true;
        }

        foreach (var layoutElement in layoutElementList)
        {
            layoutElement.enabled = true;
        }
    }
    
    public void DisableAllLayoutComponents()
    {
        if (gridLayoutGroup is not null)
        {
            gridLayoutGroup.enabled = false;
        }

        if (verticalLayoutGroup is not null)
        {
            verticalLayoutGroup.enabled = false;
        }

        if (horizontalLayoutGroup is not null)
        {
            horizontalLayoutGroup.enabled = false;
        }

        foreach (var layoutElement in layoutElementList)
        {
            layoutElement.enabled = false;
        }
    }

    private void InitializeLayoutElementList()
    {
        layoutElementList.Clear();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out LayoutElement layoutElement))
            {
                layoutElementList.Add(layoutElement);
            }
        }
    }
}