using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWayUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void Show()
    {
        panel.SetActive(true);
    }
    public void Hide()
    {
        panel.SetActive(false);
    }
}
