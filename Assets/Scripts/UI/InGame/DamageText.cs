using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public TMP_Text text;
    private Action<DamageText> returnAction;

    private float moveSpeed = 1f;    // 위로 떠오르는 속도
    private float fadeSpeed = 2f;    // 투명해지는 속도

    private CanvasGroup canvasGroup; // 알파 조절용

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Initialize(float damage, Action<DamageText> onReturn)
    {
        text.text = ((int)damage).ToString();
        returnAction = onReturn;

        // 초기화
        transform.localScale = Vector3.one;
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        StartCoroutine(FloatingCoroutine());
    }

    private IEnumerator FloatingCoroutine() // 위로 서서히 사라지는 연출
    {
        float time = 0f;
        Vector3 startPos = transform.position;

        while (time < 1f)
        {
            time += Time.deltaTime;

            transform.position = startPos + Vector3.up * moveSpeed * time;

            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, time);

            yield return null;
        }

        returnAction?.Invoke(this);
    }
}
