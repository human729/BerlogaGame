using UnityEngine;

public class TraditionButtons : MonoBehaviour
{
    [Header("������")]
    public RoomTextController controller; // ����� �� RoomPanel
    public RoomPanelUI roomUI;            // ����� �� RoomPanel
    public GameObject mainMenuPanel;      // ������ � 3 ��������
    public GameObject roomPanel;          // ������ ������� (2 ������ + 1 ������)

    /// <summary>������ �������� �� roomId.</summary>
    public void StartTradition(int roomId)
    {
        if (controller == null)
        {
            Debug.LogError("[TraditionButtons] �� ����� RoomTextController.");
            return;
        }

        // ����������� ������
        if (roomPanel) roomPanel.SetActive(true);
        if (mainMenuPanel) mainMenuPanel.SetActive(false);

        // ���� ���� RoomPanelUI � ���������� ��� ������
        if (roomUI != null)
        {
            roomUI.BeginRoom(roomId);
        }
        else
        {
            // ������: ������ �������� ����������� ����� ����������
            controller.SetRoom(roomId);
            controller.ShowGreeting();
        }
    }

    // ������ ��� ������
    public void StartDoor1() => StartTradition(1);
    public void StartDoor2() => StartTradition(2);
    public void StartDoor3() => StartTradition(3);
}
