using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BtnPanel2 : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;
    public Button continueButton;
    public GameObject panelToClose;
    public GameObject panelToOpen;

    [Header("Data")]
    public string resourceName = "data";
    public Data data;

    [Header("Typing Animation")]
    [Tooltip("Символов в секунду для эффекта печати слева направо")]
    public float charsPerSecond = 40f;
    [Tooltip("Включить авто-перенос/оверфлоу для длинных текстов")]
    public bool configureTMP = true;

    private bool secondShown;
    private Coroutine typingRoutine;
    private string currentFullText = string.Empty; // что именно печатаем сейчас

    void Awake()
    {
        secondShown = false;
        if (panelToOpen) panelToOpen.SetActive(false);
        if (panelToClose) panelToClose.SetActive(true);
    }

    void Start()
    {
        // Загрузка JSON
        var ta = Resources.Load<TextAsset>(resourceName);
        if (!ta) { Debug.LogError("[BtnPanel2] Нет Resources/" + resourceName + ".json"); return; }

        try { data = JsonConvert.DeserializeObject<Data>(ta.text); }
        catch (System.Exception e) { Debug.LogError("[BtnPanel2] Ошибка парсинга: " + e.Message); return; }

        if (data == null || data.main_menu == null) { Debug.LogError("[BtnPanel2] data/main_menu == null"); return; }

        if (configureTMP && textComponent)
        {
            textComponent.enableWordWrapping = true;
            textComponent.overflowMode = TextOverflowModes.Overflow;
        }

        // Показ ТОЛЬКО первой части с анимацией
        ShowTextAnimated(data.main_menu.arrive);

        // Кнопка
        if (continueButton)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinueClicked);
        }
    }

    void OnContinueClicked()
    {
        // Если в данный момент идёт печать — доскакать до конца одним кликом
        if (typingRoutine != null)
        {
            SkipTyping();
            return;
        }

        if (!secondShown)
        {
            // 1-й клик → анимировать вторую часть
            secondShown = true;
            ShowTextAnimated(data.main_menu.arrive2);
        }
        else
        {
            // 2-й клик → переключить панели
            if (panelToClose) panelToClose.SetActive(false);
            if (panelToOpen) panelToOpen.SetActive(true);
        }
    }

    /// <summary>
    /// Запускает анимацию печати слева направо (по символам).
    /// Поддерживает rich-text TMP — теги не ломают счётчик.
    /// </summary>
    void ShowTextAnimated(string fullText)
    {
        if (!textComponent) return;

        currentFullText = fullText ?? string.Empty;

        // Сбрасываем и готовим TMP
        textComponent.text = currentFullText;
        textComponent.ForceMeshUpdate();
        textComponent.maxVisibleCharacters = 0;

        // Гарантированно останавливаем предыдущую корутину
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeCharacters());
    }

    IEnumerator TypeCharacters()
    {
        // Количество видимых графем/символов в текущем сгенерированном меше
        int total = textComponent.textInfo.characterCount;

        // Если скорость <= 0 — показать мгновенно
        if (charsPerSecond <= 0f)
        {
            textComponent.maxVisibleCharacters = total;
            typingRoutine = null;
            yield break;
        }

        float delay = 1f / charsPerSecond;

        for (int i = 0; i <= total; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSecondsRealtime(delay);
        }

        typingRoutine = null; // закончено
    }

    /// <summary>
    /// Мгновенно дорисовывает текст до конца (удобно при повторном клике).
    /// </summary>
    void SkipTyping()
    {
        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            typingRoutine = null;
        }
        textComponent.ForceMeshUpdate();
        textComponent.maxVisibleCharacters = textComponent.textInfo.characterCount;
    }
}
