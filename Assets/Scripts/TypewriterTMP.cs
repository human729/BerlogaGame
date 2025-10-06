using System.Collections;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class TypewriterTMP : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float charsPerSecond = 40f;

    private Coroutine typing;

    void Reset()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void Play(string fullText)
    {
        if (!textComponent) textComponent = GetComponent<TextMeshProUGUI>();
        if (!textComponent) return;

        textComponent.enableAutoSizing = false;
        textComponent.enableWordWrapping = true;
        textComponent.overflowMode = TextOverflowModes.Overflow;

        textComponent.text = fullText ?? string.Empty;
        textComponent.ForceMeshUpdate();
        textComponent.maxVisibleCharacters = 0;

        if (typing != null) StopCoroutine(typing);
        typing = StartCoroutine(TypeRoutine());
    }

    public void Complete()
    {
        if (typing != null)
        {
            StopCoroutine(typing);
            typing = null;
        }
        if (!textComponent) return;
        textComponent.ForceMeshUpdate();
        textComponent.maxVisibleCharacters = textComponent.textInfo.characterCount;
    }

    private IEnumerator TypeRoutine()
    {
        if (!textComponent) yield break;

        int total = textComponent.textInfo.characterCount;

        if (charsPerSecond <= 0f)
        {
            textComponent.maxVisibleCharacters = total;
            typing = null;
            yield break;
        }

        float delay = 1f / Mathf.Max(1f, charsPerSecond);

        for (int i = 0; i <= total; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSecondsRealtime(delay);
        }
        typing = null;
    }

    public bool IsTyping => typing != null;
}
