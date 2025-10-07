using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class RoomPanelUI : MonoBehaviour
{
    [Header("UI-������")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public Button nextButton;
    public TypewriterTMP typewriter;

    [Header("���������")]
    public TextAsset roomsJson; // ���� JSON (����� �������� � ����������)
    public int startRoomId = 1; // ����� ������� �� ���������

    private List<string> _allMessages = new List<string>();
    private int _currentIndex = 0;
    private RoomsData _rooms;
    private CharacterController characterController;
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
    {   characterController.enabled = false;
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
            Debug.LogError("�� ����� ���� JSON!");
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
        if(nextButton.GetComponentInChildren<TextMeshProUGUI>().text == "���������")
        {
            gameObject.SetActive(false);
            characterController.enabled = true;
        }

        if (!nextButton) return;
        var label = nextButton.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null) label.text = txt;
    }

    /// <summary>��������� ��������� �������.</summary>
    public void BeginRoom(int roomId)
    {
        if (_rooms == null || _rooms.rooms == null)
        {
            Debug.LogError("��� ������ � ��������!");
            return;
        }

        var room = _rooms.rooms.Find(r => r.room_id == roomId);
        if (room == null)
        {
            Debug.LogError($"������� {roomId} �� �������!");
            return;
        }


        // �������� ��� ��������� �� �������
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
            _allMessages.Add("[� ���� ������� ��� ���������]");

        _currentIndex = 0;
        ShowText(_allMessages[_currentIndex]);
        SetButtonLabel("�����");
    }

    private void OnNextClicked()
    {
        if (typewriter && typewriter.IsTyping)
        {
            typewriter.Complete();
            return;
        }

        _currentIndex++;

        if (_currentIndex < _allMessages.Count)
        {
            ShowText(_allMessages[_currentIndex]);

            if (_currentIndex == _allMessages.Count - 3)
                SetButtonLabel("���������");
            else
                SetButtonLabel("�����");
        }
        else
        {
            ShowText("[������� ���������]");
            _currentIndex = -1; // �����
        }
    }
}

