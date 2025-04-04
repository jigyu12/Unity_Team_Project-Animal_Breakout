using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutGroupController : MonoBehaviour
{
    private GridLayoutGroup gridLayoutGroup;
    private VerticalLayoutGroup verticalLayoutGroup;
    private HorizontalLayoutGroup horizontalLayoutGroup;

    [SerializeField] private List<LayoutElement> layoutElementList;

    private void Start()
    {
        TryGetComponent(out gridLayoutGroup);
        TryGetComponent(out verticalLayoutGroup);
        TryGetComponent(out horizontalLayoutGroup);
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
}