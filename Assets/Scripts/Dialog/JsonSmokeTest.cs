using UnityEngine;
using Newtonsoft.Json;
using BearLab.Story;

public class JsonSmokeTest : MonoBehaviour
{
    public string resourceName = "data";

    void Start()
    {
        var ta = Resources.Load<TextAsset>(resourceName);
        if (!ta)
        {
            Debug.LogError($"[JsonSmokeTest] НЕ найден Resources/{resourceName}.json");
            return;
        }

        Debug.Log($"[JsonSmokeTest] TextAsset найден. Длина: {ta.text?.Length ?? 0} символов");

        try
        {
            var data = JsonConvert.DeserializeObject<Data>(ta.text);
            if (data == null) { Debug.LogError("[JsonSmokeTest] data == null после десериализации"); return; }

            Debug.Log($"[JsonSmokeTest] intro: {(string.IsNullOrEmpty(data.intro) ? "<пусто>" : "OK")}");
            Debug.Log($"[JsonSmokeTest] main_menu: {(data.main_menu != null ? "OK" : "null")}");
            Debug.Log($"[JsonSmokeTest] final_prompt: {data.main_menu?.final_prompt}");
            Debug.Log($"[JsonSmokeTest] rooms.Count: {data.rooms?.Count ?? -1}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("[JsonSmokeTest] Ошибка парсинга: " + e);
        }
    }
}
