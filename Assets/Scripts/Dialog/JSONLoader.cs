using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using BearLab.Story;
public class JSONLoader : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Data data; // оставим public, чтобы видеть результат в инспекторе

    void Start()
    {
        if (textComponent == null)
        {
            Debug.LogError("[JSONLoader] textComponent не назначен.");
            return;
        }

        // файл должен лежать: Assets/Resources/data.json
        TextAsset ta = Resources.Load<TextAsset>("data");
        if (ta == null)
        {
            Debug.LogError("[JSONLoader] Не найден Assets/Resources/data.json");
            return;
        }

        try
        {
            // важно: НЕ создавать локальную переменную "Data data"
            data = JsonConvert.DeserializeObject<Data>(ta.text);
        }
        catch (System.Exception e)
        {
            Debug.LogError("[JSONLoader] Ошибка парсинга: " + e.Message);
            return;
        }

        if (data == null)
        {
            Debug.LogError("[JSONLoader] Десериализация вернула null.");
            return;
        }

        // Поставь, что хочешь выводить — intro или arrive
        // textComponent.text = data.intro;
        textComponent.text = data.intro;
    
    }
}
