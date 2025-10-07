using TMPro;
using UnityEngine;

public class TextMeshProChanger : MonoBehaviour
{
    public JSONLoader jsonLoader;
    public TMP_Text textComponent;

    void Start()
    {
        // �������� ������ �� JSONLoader
        if (jsonLoader != null && jsonLoader.data != null)
        {
            // ������������� ����� �� JSON
            textComponent.text = jsonLoader.data.intro;
        }
        else
        {
            Debug.LogError("JSONLoader ��� ������ �� ����������������.");
        }
    }

    public void ChangeText(string newText)
    {
        textComponent.text = newText;
    }
}