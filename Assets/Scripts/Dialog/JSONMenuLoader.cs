using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using BearLab.Story;
public class JSONMenuLoader : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;     // куда выводим arrive/arrive2/описания
    public Button continueButton;              // кнопка "Далее" (показать вторую часть)
    public Transform choicesContainer;         // контейнер (Vertical Layout Group)
    public Button choiceButtonPrefab;          // префаб кнопки выбора (с TMP внутри)
    public bool buildChoicesAfterSecond = true;

    [Header("Data")]
    public string resourceName = "data";       // Assets/Resources/data.json
    public Data data;

    bool secondShown = false;

    void Start()
    {
        if (!textComponent) { Debug.LogError("[JSONMenuLoader] textComponent не назначен."); return; }

        var ta = Resources.Load<TextAsset>(resourceName);
        if (!ta) { Debug.LogError($"[JSONMenuLoader] Не найден Resources/{resourceName}.json"); return; }

        try { data = JsonConvert.DeserializeObject<Data>(ta.text); }
        catch (System.Exception e) { Debug.LogError("[JSONMenuLoader] Ошибка парсинга: " + e.Message); return; }

        if (data?.main_menu == null) { Debug.LogError("[JSONMenuLoader] main_menu == null."); return; }

        // 1) Первая часть
        ShowFirstPart();

        // 2) Подписываем кнопку
        if (continueButton)
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ShowSecondPart);
        }
    }

    void ShowFirstPart()
    {
        textComponent.enableWordWrapping = true;
        textComponent.overflowMode = TextOverflowModes.Overflow;

        textComponent.text = data.main_menu.arrive ?? data.intro ?? "";
        secondShown = false;

        // если были старые кнопки — очистим
        ClearChoices();
    }

    void ShowSecondPart()
    {
        if (secondShown) return;

        textComponent.text = data.main_menu.arrive2 ?? "";
        secondShown = true;

        if (buildChoicesAfterSecond)
            BuildChoices();
    }

    void BuildChoices()
    {
        if (!choicesContainer || !choiceButtonPrefab || data.main_menu.choices == null) return;

        ClearChoices();

        foreach (var ch in data.main_menu.choices)
        {
            var btn = Instantiate(choiceButtonPrefab, choicesContainer);
            var label = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (label) label.text = $"{ch.door_number}. {ch.tradition_name}";
            btn.onClick.AddListener(() => OnChoiceClicked(ch));
        }
    }

    void ClearChoices()
    {
        if (!choicesContainer) return;
        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
            Destroy(choicesContainer.GetChild(i).gameObject);
    }

    void OnChoiceClicked(Choice ch)
    {
        // тут можно переключать панели/сцены; пока выводим описание
        textComponent.text = ch.description;
        Debug.Log($"[JSONMenuLoader] Выбрано: {ch.door_number} — {ch.tradition_name}");
    }
}
    