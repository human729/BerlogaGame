using System.Linq;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;

public class RoomInfoPresenter : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI titleText;   // "� � <��������>"
    public TextMeshProUGUI bodyText;    // start_message
    public TypewriterTMP typewriter;    // �����������

    [Header("Data")]
    public string resourceName = "data"; // Assets/Resources/data.json
    public int roomIdOnStart = 0;        // 0 � ������ ��� ���������
    public Data data;

    void Awake()
    {
        // ������ ������ �� ����� ������� �� ����������
        var ta = Resources.Load<TextAsset>(resourceName);
        data = JsonConvert.DeserializeObject<Data>(ta.text);

        // ������� ��������� TMP (��� Auto Size)
        if (titleText) { titleText.enableAutoSizing = false; titleText.enableWordWrapping = true; titleText.overflowMode = TMPro.TextOverflowModes.Overflow; }
        if (bodyText) { bodyText.enableAutoSizing = false; bodyText.enableWordWrapping = true; bodyText.overflowMode = TMPro.TextOverflowModes.Overflow; }
    }

    void OnEnable()
    {
        if (roomIdOnStart > 0)
            ShowRoomById(roomIdOnStart);
    }

    // ��������� ���� ���
    public void ShowByDoorNumber(int doorNumber) => ShowRoomById(doorNumber);

    public void ShowRoomById(int id)
    {
        var room = data.rooms.First(r => r.room_id == id);

        if (titleText) titleText.text = $"� � {room.tradition}";
        if (!bodyText) return;

        if (typewriter)
        {
            typewriter.textComponent = bodyText;
            typewriter.Play(room.start_message);
        }
        else
        {
            bodyText.text = room.start_message;
        }
    }

    // �������� ��� ������ ������ (���� �����)
    public void ShowPervoprohodcy() => ShowRoomById(1);
    public void ShowKonstruktory() => ShowRoomById(2);
    public void ShowProgrammisty() => ShowRoomById(3);
}
