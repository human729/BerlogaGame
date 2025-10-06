using UnityEngine;

public class TraditionButtons : MonoBehaviour
{
    [Header("Ссылки")]
    public RoomTextController controller; // висит на RoomPanel
    public RoomPanelUI roomUI;            // висит на RoomPanel
    public GameObject mainMenuPanel;      // панель с 3 кнопками
    public GameObject roomPanel;          // панель комнаты (2 текста + 1 кнопка)

    /// <summary>Запуск традиции по roomId.</summary>
    public void StartTradition(int roomId)
    {
        if (controller == null)
        {
            Debug.LogError("[TraditionButtons] Не задан RoomTextController.");
            return;
        }

        // Переключаем панели
        if (roomPanel) roomPanel.SetActive(true);
        if (mainMenuPanel) mainMenuPanel.SetActive(false);

        // Если есть RoomPanelUI — используем его циклер
        if (roomUI != null)
        {
            roomUI.BeginRoom(roomId);
        }
        else
        {
            // Фолбэк: просто показать приветствие через контроллер
            controller.SetRoom(roomId);
            controller.ShowGreeting();
        }
    }

    // Обёртки под кнопки
    public void StartDoor1() => StartTradition(1);
    public void StartDoor2() => StartTradition(2);
    public void StartDoor3() => StartTradition(3);
}
