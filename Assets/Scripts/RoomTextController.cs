using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking; // для UnityWebRequest (Android)
#if TMP_PRESENT || TEXTMESHPRO
using TMPro;
#endif
using Newtonsoft.Json;

[Serializable]
public class RoomTask
{
    public int task_number;
    public string message;
    public string after_passing;
}

[Serializable]
public class RoomData
{
    public int room_id;
    public string name;           // отображаемое имя/традиция
    public string start_message;  // приветствие
    public List<RoomTask> rooms_tasks;
}

[Serializable]
public class RoomsRoot
{
    public List<RoomData> rooms;
}

/// <summary>
/// Загружает комнаты из Resources/rooms.json,
/// хранит текущую комнату и предоставляет геттеры для UI (RoomPanelUI).
/// </summary>
public class RoomTextController : MonoBehaviour
{
    [Header("Источник данных")]
    [Tooltip("Файл JSON в Resources")]
    public string jsonFileName = "rooms.json";

    [Tooltip("Какую комнату открыть по умолчанию (необязательно)")]
    public int startRoomId = 1;

#if TMP_PRESENT || TEXTMESHPRO
    [Header("UI (опционально, можно не назначать)")]
    public TMP_Text targetText;
#endif

    [Header("Поведение (только для встроенного вывода)")]
    [Tooltip("Очищать ли буфер перед каждым новым сообщением")]
    public bool clearBeforeEachMessage = false;

    private RoomsRoot _root;
    private RoomData _current;

    private readonly StringBuilder _buffer = new StringBuilder();

    private void Start()
    {
        StartCoroutine(InitFlow());
    }

    private IEnumerator InitFlow()
    {
        yield return StartCoroutine(LoadJsonCoroutine());
        if (_root == null || _root.rooms == null || _root.rooms.Count == 0)
        {
            Debug.LogError("[RoomTextController] Данные не загружены или пусты.");
            yield break;
        }

        // опционально: выбрать комнату по умолчанию
        if (startRoomId != 0)
            SetRoom(startRoomId);
    }

    // Универсальная загрузка JSON (Editor/Standalone — прямой файл, Android — UWR)
    private IEnumerator LoadJsonCoroutine()
    {
        // имя без расширения — Resources ищет по "rooms", не "rooms.json"
        string resourceName = Path.GetFileNameWithoutExtension(jsonFileName);
        TextAsset ta = Resources.Load<TextAsset>(resourceName);
        if (ta == null)
        {
            Debug.LogError($"[RoomTextController] Resources не нашёл: {resourceName}");
            yield break;
        }
        ParseJson(ta.text);
        yield return null;
    }
    private void ParseJson(string json)
    {
        try
        {
            _root = JsonConvert.DeserializeObject<RoomsRoot>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[RoomTextController] Ошибка чтения JSON: {e}");
        }
    }

    // ---------- ПУБЛИЧНЫЕ API, которые дергает RoomPanelUI ----------

    public void SetRoom(int roomId)
    {
        if (_root?.rooms == null)
        {
            Debug.LogError("[RoomTextController] Данные ещё не загружены.");
            return;
        }
        _current = _root.rooms.Find(r => r.room_id == roomId);
        if (_current == null)
        {
            Debug.LogError($"[RoomTextController] Комната {roomId} не найдена.");
            return;
        }

        // опционально: синхронизировать встроенный буфер/целевой TMP
        _buffer.Clear();
        ApplyText(); // просто обновит targetText/лог, если назначен
    }

    /// Отображаемое имя (верхний текст)
    public string GetCurrentDisplayName()
    {
        if (_current == null) return "";
        return !string.IsNullOrWhiteSpace(_current.name)
            ? _current.name
            : $"Комната {_current.room_id}";
    }

    /// Приветствие (первое сообщение)
    public string GetGreetingText() => _current?.start_message ?? "";

    /// Кол-во задач в комнате
    public int GetTaskCount() => _current?.rooms_tasks?.Count ?? 0;

    /// Текст задачи по индексу (0..n-1). Если message пустой — подставим плейсхолдер.
    public string GetTaskMessageByIndex(int index)
    {
        if (_current?.rooms_tasks == null || index < 0 || index >= _current.rooms_tasks.Count) return "";
        var t = _current.rooms_tasks[index];
        return string.IsNullOrWhiteSpace(t.message) ? $"[Задача {t.task_number}]" : t.message;
    }

    /// Текст “после прохождения” по индексу (0..n-1)
    public string GetTaskAfterByIndex(int index)
    {
        if (_current?.rooms_tasks == null || index < 0 || index >= _current.rooms_tasks.Count) return "";
        return _current.rooms_tasks[index].after_passing ?? "";
    }

    // ---------- Необязательный внутренний вывод (если привязан targetText) ----------

    /// Показать приветствие во внутренний буфер/targetText (UI RoomPanelUI это не использует)
    public void ShowGreeting()
    {
        if (_current == null) return;

        if (clearBeforeEachMessage) _buffer.Clear();
        _buffer.AppendLine(GetCurrentDisplayName() + ":");
        _buffer.AppendLine(GetGreetingText());
        _buffer.AppendLine();

        ApplyText();
    }

    /// Показать текст задачи по номеру (не индекс!), во внутренний буфер/targetText
    public void ShowTaskMessage(int taskNumber)
    {
        if (_current?.rooms_tasks == null) return;

        var task = _current.rooms_tasks.Find(t => t.task_number == taskNumber);
        if (task == null)
        {
            Debug.LogWarning($"[RoomTextController] Нет задачи #{taskNumber} в комнате {_current?.room_id}");
            return;
        }

        if (clearBeforeEachMessage) _buffer.Clear();
        _buffer.AppendLine(string.IsNullOrWhiteSpace(task.message) ? $"[Задача {task.task_number}]" : task.message);

        ApplyText();
    }

    /// Показать текст after_passing по номеру задачи, во внутренний буфер/targetText
    public void ShowTaskAfterPassing(int taskNumber)
    {
        if (_current?.rooms_tasks == null) return;

        var task = _current.rooms_tasks.Find(t => t.task_number == taskNumber);
        if (task == null) return;

        if (clearBeforeEachMessage) _buffer.Clear();
        if (!string.IsNullOrWhiteSpace(task.after_passing))
            _buffer.AppendLine(task.after_passing);

        ApplyText();
    }

    private void ApplyText()
    {
#if TMP_PRESENT || TEXTMESHPRO
        if (targetText != null) targetText.text = _buffer.ToString();
#endif
        if (_buffer.Length > 0)
            Debug.Log(_buffer.ToString());
    }

    // Если не используешь встроенный “пропуск”, этот корутин можно не вызывать
    private IEnumerator WaitToSkip()
    {
        _buffer.AppendLine("(Нажми любую клавишу/ЛКМ/тап, чтобы продолжить…)");
        ApplyText();

        while (!(Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
            yield return null;

        if (clearBeforeEachMessage)
            _buffer.Clear();
        else
            _buffer.AppendLine();

        _buffer.AppendLine("[Дальше управление задачами — внешним кодом/переменными]");
        ApplyText();
    }
}
