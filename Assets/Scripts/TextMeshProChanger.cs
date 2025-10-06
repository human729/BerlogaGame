using TMPro;
using UnityEngine;

public class TextMeshProChanger : MonoBehaviour
{
    public JSONLoader jsonLoader;
    public TMP_Text textComponent;

    void Start()
    {
        // ѕолучаем данные из JSONLoader
        if (jsonLoader != null && jsonLoader.data != null)
        {
            // ”станавливаем текст из JSON
            textComponent.text = jsonLoader.data.intro;
        }
        else
        {
            Debug.LogError("JSONLoader или данные не инициализированы.");
        }
    }

    public void ChangeText(string newText)
    {
        textComponent.text = newText;
    }
}