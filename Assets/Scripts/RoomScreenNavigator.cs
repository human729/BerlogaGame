using UnityEngine;

public class RoomScreenNavigator : MonoBehaviour
{
    [System.Serializable]
    public struct RoomPanelBinding
    {
        [Tooltip("room_id �� JSON: 1 � �������������, 2 � ������������, 3 � ������������")]
        public int doorNumber;

        [Tooltip("������ (GameObject) ���������� ��������")]
        public GameObject panel;

        [Tooltip("Presenter �� ���� ������ (������� ���������/����� � ���������)")]
        public RoomInfoPresenter presenter;
    }

    [Header("Panels")]
    [Tooltip("������� ���� � �������� ������ ��������")]
    public GameObject menuPanel;

    [Tooltip("������ ������� �� ���������")]
    public RoomPanelBinding[] rooms;

    // ���������� �� OnClick ������ ���� (������� 1/2/3)
    public void OpenRoomByDoor(int doorNumber)
    {
        if (!menuPanel || rooms == null || rooms.Length == 0)
        {
            Debug.LogError("[RoomScreenNavigator] �� ��������� ������ � ����������.");
            return;
        }

        // ��������� ����
        menuPanel.SetActive(false);

        // ��������� ��� ������ ������
        CloseAllRooms();

        // ���� ������ ������ �� ������ �����
        bool opened = false;
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].doorNumber == doorNumber && rooms[i].panel != null)
            {
                rooms[i].panel.SetActive(true);
                if (rooms[i].presenter != null)
                {
                    rooms[i].presenter.ShowByDoorNumber(doorNumber);
                }
                else
                {
                    Debug.LogWarning($"[RoomScreenNavigator] Panel {doorNumber} ��� RoomInfoPresenter � ����� �� ����� ������� �������������.");
                }
                opened = true;
                break;
            }
        }

        if (!opened)
        {
            Debug.LogWarning($"[RoomScreenNavigator] �� ������� ������ ��� doorNumber={doorNumber}. ����������� � ����.");
            BackToMenu();
        }
    }

    // ������ "�����" �� ������ ����� �������
    public void BackToMenu()
    {
        // ��������� ��� ������ ������
        CloseAllRooms();

        // ���������� ����
        if (menuPanel) menuPanel.SetActive(true);
    }

    public void CloseAllRooms()
    {
        if (rooms == null) return;
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].panel) rooms[i].panel.SetActive(false);
        }
    }

    // �����������: ������� �������������/�������������/�������������� ������� ��������
    public void OpenPervoprohodcy() => OpenRoomByDoor(1);
    public void OpenKonstruktory() => OpenRoomByDoor(2);
    public void OpenProgrammisty() => OpenRoomByDoor(3);
}
