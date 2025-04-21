#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;
using UnityEngine;

public class PlayerStatusDebug : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private GameUIManager gameUIManager;
    [SerializeField] private PlayerMove playerMove;

    [SerializeField] private bool showDebug = true;

    void Update()
    {
        if (!showDebug || playerStatus == null) return;

        string debugText = "";
        if (playerMove.isJumping) debugText += "점프중\n";
        if (playerStatus.IsReviving) debugText += "부활중\n";
        if (playerStatus.IsInvincible) debugText += "무적중\n";

        textMesh.text = debugText.TrimEnd();
    }

    public void SetShowDebug(bool show)
    {
        showDebug = show;
        if (textMesh != null)
            textMesh.gameObject.SetActive(showDebug);
    }

#if UNITY_EDITOR
    [ContextMenu("디버그 텍스트 On")]
    private void EnableDebugText()
    {
        SetShowDebug(true);
    }

    [ContextMenu("디버그 텍스트 Off")]
    private void DisableDebugText()
    {
        SetShowDebug(false);
    }
#endif
}
