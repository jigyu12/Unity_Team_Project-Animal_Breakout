using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class LocalizationTEst : MonoBehaviour
{
    public TextMeshProUGUI text;
    public LocalizeStringEvent local;

    [ContextMenu("do")]
    public void Do()
    {
        //text.text = LocalizationManager.GetLZString(local.StringReference, 40f);
        text.text = LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, "ACHIEVEMENT_DESCRIPTION_RECORDHOLDER_EXPORT",40);
    }
}
