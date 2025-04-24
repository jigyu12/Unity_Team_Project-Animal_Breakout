using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class LocalizedTextBinder : MonoBehaviour
{
    public LocalizedString localizedString;
    [SerializeField] private TMP_Text target;

    public void OnEnable()
    {
        localizedString.StringChanged += UpdateText;
    }

    private void OnDisable()
    {
        localizedString.StringChanged -= UpdateText;
    }

    public void SetArgumentsAndRefresh(params object[] args)
    {
        localizedString.Arguments = args;
        localizedString.RefreshString(); // 값 변경 후 강제 갱신
    }

    private void UpdateText(string formattedText)
    {
        target.text = formattedText; // TMP에 최종 적용
    }
}
