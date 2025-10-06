using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using BearLab.Story;
public class JSONMenuLoader : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;     // ���� ������� arrive/arrive2/��������
    public Button continueButton;              // ������ "�����" (�������� ������ �����)
    public Transform choicesContainer;         // ��������� (Vertical Layout Group)
    public Button choiceButtonPrefab;          // ������ ������ ������ (� TMP ������)
    public bool buildChoicesAfterSecond = true;

    [Header("Data")]
    public string resourceName = "data";       // Assets/Resources/data.json
    public Data data;

    bool secondShown = false;

    void Start()
    {
        if (!textComponent) { Debug.LogError("[JSONMenuLoader] textComponent �� ��������."); return; }

        var ta = Resources.Load<TextAsset>(resourceName);
        if (!ta) { Debug.LogError($"[JSONMenuLoader] �� ������ Resources/{resourceName}.json"); return; }

        try { data = JsonConvert.DeserializeObject<Data>(ta.text); }
        catch (System.Exception e) { Debug.LogError("[JSONMenuLoader] ������ ��������: " + e.Message); return; }

        if (data?.main_menu == null) { Debug.LogError("[JSONMenuLoader] main_menu == null."); return; }

        // 1) ������ �����
        ShowFirstPart();

        // 2) ����������� ������
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

        // ���� ���� ������ ������ � �������
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
        // ��� ����� ����������� ������/�����; ���� ������� ��������
        textComponent.text = ch.description;
        Debug.Log($"[JSONMenuLoader] �������: {ch.door_number} � {ch.tradition_name}");
    }
}
    