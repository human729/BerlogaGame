using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅ пїЅпїЅпїЅпїЅпїЅпїЅ JSON, пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ.
/// </summary>
public class RoomPanelUI : MonoBehaviour
{
    [Header("UI-пїЅпїЅпїЅпїЅпїЅпїЅ")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public Button nextButton;
    public TypewriterTMP typewriter;

    [Header("пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ")]
    public TextAsset roomsJson;
    public int startRoomId = 1;

    private List<string> _allMessages = new List<string>();
    private int _currentIndex = 0;
    private RoomsData _rooms;
<<<<<<< Updated upstream:Assets/Scripts/Dialog/RoomPanelUI.cs
    [SerializeField] private CharacterController characterController;
=======
>>>>>>> Stashed changes:Assets/Scripts/RoomPanelUI.cs
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
<<<<<<< Updated upstream:Assets/Scripts/Dialog/RoomPanelUI.cs
=======
        if(nextButton.GetComponentInChildren<TextMeshProUGUI>().text == "Завершить")
        {
            gameObject.SetActive(false);
        }
>>>>>>> Stashed changes:Assets/Scripts/RoomPanelUI.cs

        if (!nextButton) return;
        var label = nextButton.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null) label.text = txt;
    }

    /// <summary>пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ.</summary>
    public void BeginRoom(int roomId)
    {
        if (_rooms == null || _rooms.rooms == null)
        {
            Debug.LogError("пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ пїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ!");
            return;
        }

        var room = _rooms.rooms.Find(r => r.room_id == roomId);
        if (room == null)
        {
            Debug.LogError($"пїЅпїЅпїЅпїЅпїЅпїЅпїЅ {roomId} пїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ!");
            return;
        }

<<<<<<< Updated upstream:Assets/Scripts/Dialog/RoomPanelUI.cs

        // пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ
=======
>>>>>>> Stashed changes:Assets/Scripts/RoomPanelUI.cs
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

        _currentIndex = 0;
<<<<<<< Updated upstream:Assets/Scripts/Dialog/RoomPanelUI.cs
        ShowText(_allMessages[_currentIndex]+".");
        SetButtonLabel("пїЅпїЅпїЅпїЅпїЅ");
=======
        ShowText(_allMessages[_currentIndex] + ".");
        print(_allMessages[_currentIndex]);
        SetButtonLabel("Далее");
>>>>>>> Stashed changes:Assets/Scripts/RoomPanelUI.cs
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
<<<<<<< Updated upstream:Assets/Scripts/Dialog/RoomPanelUI.cs
            ShowText(_allMessages[_currentIndex]+".");

            if (_currentIndex == _allMessages.Count - 1)
                SetButtonLabel("пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ");
=======
            ShowText(_allMessages[_currentIndex] + ".");

            if (_currentIndex == _allMessages.Count - 1)
                SetButtonLabel("Завершить");
>>>>>>> Stashed changes:Assets/Scripts/RoomPanelUI.cs
            else
                SetButtonLabel("пїЅпїЅпїЅпїЅпїЅ");
        }
        else
        {
<<<<<<< Updated upstream:Assets/Scripts/Dialog/RoomPanelUI.cs
            _currentIndex = -1; // пїЅпїЅпїЅпїЅпїЅ
=======
            _currentIndex = -1; // сброс
>>>>>>> Stashed changes:Assets/Scripts/RoomPanelUI.cs
        }

        if (nextButton.GetComponentInChildren<TextMeshProUGUI>().text != "Завершить")
        {
            gameObject.SetActive(false);
        }
    }
}

