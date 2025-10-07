using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking; // ��� UnityWebRequest (Android)
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
    public string name;           // ������������ ���/��������
    public string start_message;  // �����������
    public List<RoomTask> rooms_tasks;
}

[Serializable]
public class RoomsRoot
{
    public List<RoomData> rooms;
}

/// <summary>
/// ��������� ������� �� Resources/rooms.json,
/// ������ ������� ������� � ������������� ������� ��� UI (RoomPanelUI).
/// </summary>
public class RoomTextController : MonoBehaviour
{
    [Header("�������� ������")]
    [Tooltip("���� JSON � Resources")]
    public string jsonFileName = "rooms.json";

    [Tooltip("����� ������� ������� �� ��������� (�������������)")]
    public int startRoomId = 1;

#if TMP_PRESENT || TEXTMESHPRO
    [Header("UI (�����������, ����� �� ���������)")]
    public TMP_Text targetText;
#endif

    [Header("��������� (������ ��� ����������� ������)")]
    [Tooltip("������� �� ����� ����� ������ ����� ����������")]
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
            Debug.LogError("[RoomTextController] ������ �� ��������� ��� �����.");
            yield break;
        }

        // �����������: ������� ������� �� ���������
        if (startRoomId != 0)
            SetRoom(startRoomId);
    }

    // ������������� �������� JSON (Editor/Standalone � ������ ����, Android � UWR)
    private IEnumerator LoadJsonCoroutine()
    {
        // ��� ��� ���������� � Resources ���� �� "rooms", �� "rooms.json"
        string resourceName = Path.GetFileNameWithoutExtension(jsonFileName);
        TextAsset ta = Resources.Load<TextAsset>(resourceName);
        if (ta == null)
        {
            Debug.LogError($"[RoomTextController] Resources �� �����: {resourceName}");
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
            Debug.LogError($"[RoomTextController] ������ ������ JSON: {e}");
        }
    }

    // ---------- ��������� API, ������� ������� RoomPanelUI ----------

    public void SetRoom(int roomId)
    {
        if (_root?.rooms == null)
        {
            Debug.LogError("[RoomTextController] ������ ��� �� ���������.");
            return;
        }
        _current = _root.rooms.Find(r => r.room_id == roomId);
        if (_current == null)
        {
            Debug.LogError($"[RoomTextController] ������� {roomId} �� �������.");
            return;
        }

        // �����������: ���������������� ���������� �����/������� TMP
        _buffer.Clear();
        ApplyText(); // ������ ������� targetText/���, ���� ��������
    }

    /// ������������ ��� (������� �����)
    public string GetCurrentDisplayName()
    {
        if (_current == null) return "";
        return !string.IsNullOrWhiteSpace(_current.name)
            ? _current.name
            : $"������� {_current.room_id}";
    }

    /// ����������� (������ ���������)
    public string GetGreetingText() => _current?.start_message ?? "";

    /// ���-�� ����� � �������
    public int GetTaskCount() => _current?.rooms_tasks?.Count ?? 0;

    /// ����� ������ �� ������� (0..n-1). ���� message ������ � ��������� �����������.
    public string GetTaskMessageByIndex(int index)
    {
        if (_current?.rooms_tasks == null || index < 0 || index >= _current.rooms_tasks.Count) return "";
        var t = _current.rooms_tasks[index];
        return string.IsNullOrWhiteSpace(t.message) ? $"[������ {t.task_number}]" : t.message;
    }

    /// ����� ������ ������������ �� ������� (0..n-1)
    public string GetTaskAfterByIndex(int index)
    {
        if (_current?.rooms_tasks == null || index < 0 || index >= _current.rooms_tasks.Count) return "";
        return _current.rooms_tasks[index].after_passing ?? "";
    }

    // ---------- �������������� ���������� ����� (���� �������� targetText) ----------

    /// �������� ����������� �� ���������� �����/targetText (UI RoomPanelUI ��� �� ����������)
    public void ShowGreeting()
    {
        if (_current == null) return;

        if (clearBeforeEachMessage) _buffer.Clear();
        _buffer.AppendLine(GetCurrentDisplayName() + ":");
        _buffer.AppendLine(GetGreetingText());
        _buffer.AppendLine();

        ApplyText();
    }

    /// �������� ����� ������ �� ������ (�� ������!), �� ���������� �����/targetText
    public void ShowTaskMessage(int taskNumber)
    {
        if (_current?.rooms_tasks == null) return;

        var task = _current.rooms_tasks.Find(t => t.task_number == taskNumber);
        if (task == null)
        {
            Debug.LogWarning($"[RoomTextController] ��� ������ #{taskNumber} � ������� {_current?.room_id}");
            return;
        }

        if (clearBeforeEachMessage) _buffer.Clear();
        _buffer.AppendLine(string.IsNullOrWhiteSpace(task.message) ? $"[������ {task.task_number}]" : task.message);

        ApplyText();
    }

    /// �������� ����� after_passing �� ������ ������, �� ���������� �����/targetText
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

    // ���� �� ����������� ���������� ��������, ���� ������� ����� �� ��������
    private IEnumerator WaitToSkip()
    {
        _buffer.AppendLine("(����� ����� �������/���/���, ����� �����������)");
        ApplyText();

        while (!(Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
            yield return null;

        if (clearBeforeEachMessage)
            _buffer.Clear();
        else
            _buffer.AppendLine();

        _buffer.AppendLine("[������ ���������� �������� � ������� �����/�����������]");
        ApplyText();
    }
}
