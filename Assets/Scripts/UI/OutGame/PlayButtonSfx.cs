using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButtonSfx : MonoBehaviour
{
    private Button buttonToPlaySfx;
    [SerializeField] SfxClipId sfxClipId;

    private void Start()
    {
        TryGetComponent(out buttonToPlaySfx);
        
        buttonToPlaySfx.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySfx(sfxClipId);
        });
    }
    
    private void OnDestroy()
    {
        buttonToPlaySfx.onClick.RemoveAllListeners();
    }
}