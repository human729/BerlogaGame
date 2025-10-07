using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{
    public Text textComponent;

    void Start()
    {
        // Изменяем текст при старте
        textComponent.text = "Новый текст";
    }

    public void ChangeText(string newText)
    {
        // Метод для изменения текста
        textComponent.text = newText;
    }
}