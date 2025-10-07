using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{
    public Text textComponent;

    void Start()
    {
        // �������� ����� ��� ������
        textComponent.text = "����� �����";
    }

    public void ChangeText(string newText)
    {
        // ����� ��� ��������� ������
        textComponent.text = newText;
    }
}