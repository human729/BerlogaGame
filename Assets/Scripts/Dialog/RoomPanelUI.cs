using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class RoomPanelUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public Button nextButton;
    public TypewriterTMP typewriter;

    public TextAsset roomsJson;
    public int startRoomId = 1;

    private List<string> _allMessages = new List<string>();
    private int _currentIndex = 0;
    private RoomsData _rooms;

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
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextClicked);

        if (!typewriter && bodyText)
            typewriter = bodyText.GetComponent<TypewriterTMP>();

        LoadJson();
        BeginRoom(startRoomId);
        
    }

    private void LoadJson()
    {
        if (roomsJson == null)
        {
            Debug.LogError("пїЅпїЅ пїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅ JSON!");
            return;
        }

        _rooms = JsonUtility.FromJson<RoomsData>(roomsJson.text);
    }

    private void ShowText(string txt)
    {
        if (typewriter) typewriter.Play(txt);
        else if (bodyText) bodyText.text = txt;
    }

    private void SetButtonLabel(string txt)
    {
        if(nextButton.GetComponentInChildren<TextMeshProUGUI>().text == "Завершить")
        {
            gameObject.SetActive(false);
        }
        if (!nextButton) return;
        var label = nextButton.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null) label.text = txt;
    }

    public void BeginRoom(int roomId)
    {
        if (_rooms == null || _rooms.rooms == null)
        {
            return;
        }

        var room = _rooms.rooms.Find(r => r.room_id == roomId);
        if (room == null)
        {
            return;
        }

        _allMessages.Clear();

        List<string> messages = room.start_message.Split(".").ToList();

        foreach (string message in messages)
        {
            if (!string.IsNullOrEmpty(message)) _allMessages.Add(message);
        }

        if (room.rooms_tasks != null)
        {
            foreach (var task in room.rooms_tasks)
            {
                if (!string.IsNullOrEmpty(task.message)) _allMessages.Add(task.message);
                if (!string.IsNullOrEmpty(task.after_passing)) _allMessages.Add(task.after_passing);
            }
        }

        if (_allMessages.Count == 0)
            _allMessages.Add("[пїЅ пїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ]");

        if (_allMessages.Count == 1)
            SetButtonLabel("Завершить");

        if (_allMessages.Count == 1)
            SetButtonLabel("Завершить");

        _currentIndex = 0;
        ShowText(_allMessages[_currentIndex] + ".");
        print(_allMessages[_currentIndex]);
        SetButtonLabel("Далее");
    }
        
    private void OnNextClicked()
    {
        if (nextButton.GetComponentInChildren<TextMeshProUGUI>().text == "пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ")
        {
            gameObject.SetActive(false);
        }

        if (typewriter && typewriter.IsTyping)
        {
            typewriter.Complete();
            return;
        }

        _currentIndex++;

        if (_currentIndex < _allMessages.Count)
        {
            ShowText(_allMessages[_currentIndex] + ".");

            if (_currentIndex == _allMessages.Count - 1)
                SetButtonLabel("Завершить");
            else
                SetButtonLabel("пїЅпїЅпїЅпїЅпїЅ");
        }
        else
        {
            _currentIndex = -1; // сброс
        }

        if (nextButton.GetComponentInChildren<TextMeshProUGUI>().text != "Завершить")
        {
            gameObject.SetActive(false);
        }
    }
}

