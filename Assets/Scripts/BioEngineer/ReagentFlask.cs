using UnityEngine;
using UnityEngine.UI;

public class ReagentFlask : MonoBehaviour
{
    [Header("Reagent Properties")]
    public string elementName;
    public Color elementColor;

    [Header("UI References")]
    public Text elementText;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnFlaskClick);

        // Устанавливаем текст элемента
        if (elementText != null)
        {
            elementText.text = elementName;
        }
    }

    private void OnFlaskClick()
    {
        MixingController.Instance.AddReagentToMainFlask(elementName, elementColor);
    }
}