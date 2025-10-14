using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class RoomPanelUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public Button nextButton;
    public TypewriterTMP typewriter;

    [Header("Data")]
    public TextAsset roomsJson;
    public int startRoomId = 1;

    private List<string> _allMessages = new List<string>();
    private int _currentIndex = 0;
    private RoomsData _rooms;

    [System.Serializable]
    public class TaskData
    {
        public int task_number;
        public string message;
        public string after_passing;
    }

    [System.Serializable]
    public class RoomData
    {
        public int room_id;
        public string name;
        public string start_message;
        public List<TaskData> rooms_tasks;
    }

    [System.Serializable]
    public class RoomsData
    {
        public List<RoomData> rooms;
    }

    private void Awake()
    {
        nextButton?.onClick.AddListener(OnNextClicked);

        if (!typewriter && bodyText)
            typewriter = bodyText.GetComponent<TypewriterTMP>();

        LoadJson();
        BeginRoom(startRoomId);
    }

    private void LoadJson()
    {
        if (roomsJson == null)
        {
            Debug.LogError("Rooms JSON file not assigned!");
            return;
        }

        try
        {
            _rooms = JsonUtility.FromJson<RoomsData>(roomsJson.text);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse JSON: {e.Message}");
        }
    }

    private void ShowText(string text)
    {
        if (typewriter)
            typewriter.Play(text);
        else if (bodyText)
            bodyText.text = text;
    }

    private void UpdateButtonState()
    {
        if (!nextButton) return;

        var label = nextButton.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
        {
            if (_currentIndex >= _allMessages.Count - 1)
                label.text = "Завершить";
            else
                label.text = "Далее";
        }
    }

    public void BeginRoom(int roomId)
    {
        if (_rooms?.rooms == null)
        {
            Debug.LogError("Rooms data not loaded!");
            return;
        }

        var room = _rooms.rooms.Find(r => r.room_id == roomId);
        if (room == null)
        {
            Debug.LogError($"Room with ID {roomId} not found!");
            return;
        }

        if (titleText != null && !string.IsNullOrEmpty(room.name))
            titleText.text = room.name;

        CompileRoomMessages(room);

        _currentIndex = 0;
        ShowCurrentMessage();
        UpdateButtonState();
    }

    private void CompileRoomMessages(RoomData room)
    {
        _allMessages.Clear();

        if (!string.IsNullOrEmpty(room.start_message))
        {
            var sentences = room.start_message.Split('.');
            foreach (var sentence in sentences)
            {
                var trimmed = sentence.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    _allMessages.Add(trimmed);
            }
        }

        if (room.rooms_tasks != null)
        {
            foreach (var task in room.rooms_tasks)
            {
                if (!string.IsNullOrEmpty(task.message))
                    _allMessages.Add(task.message.Trim());

                if (!string.IsNullOrEmpty(task.after_passing))
                    _allMessages.Add(task.after_passing.Trim());
            }
        }

        if (_allMessages.Count == 0)
            _allMessages.Add("Нет сообщений");
    }

    private void ShowCurrentMessage()
    {
        if (_currentIndex < _allMessages.Count)
        {
            ShowText(_allMessages[_currentIndex] + ".");
        }
    }

    private void OnNextClicked()
    {
        if (typewriter && typewriter.IsTyping)
        {
            typewriter.Complete();
            return;
        }

        if (_currentIndex >= _allMessages.Count - 1)
        {
            gameObject.SetActive(false);
            if(SceneManager.GetActiveScene().name == "EndScene")
            {
                SceneManager.LoadScene("MainMenu");
            }
            return;
        }

        _currentIndex++;
        ShowCurrentMessage();
        UpdateButtonState();
    }
}