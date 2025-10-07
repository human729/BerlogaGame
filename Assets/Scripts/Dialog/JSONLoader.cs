using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using BearLab.Story;
public class JSONLoader : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Data data; // ������� public, ����� ������ ��������� � ����������

    void Start()
    {
        if (textComponent == null)
        {
            Debug.LogError("[JSONLoader] textComponent �� ��������.");
            return;
        }

        // ���� ������ ������: Assets/Resources/data.json
        TextAsset ta = Resources.Load<TextAsset>("data");
        if (ta == null)
        {
            Debug.LogError("[JSONLoader] �� ������ Assets/Resources/data.json");
            return;
        }

        try
        {
            // �����: �� ��������� ��������� ���������� "Data data"
            data = JsonConvert.DeserializeObject<Data>(ta.text);
        }
        catch (System.Exception e)
        {
            Debug.LogError("[JSONLoader] ������ ��������: " + e.Message);
            return;
        }

        if (data == null)
        {
            Debug.LogError("[JSONLoader] �������������� ������� null.");
            return;
        }

        // �������, ��� ������ �������� � intro ��� arrive
        // textComponent.text = data.intro;
        textComponent.text = data.intro;
    
    }
}
