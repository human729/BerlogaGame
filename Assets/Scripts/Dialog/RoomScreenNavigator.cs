using UnityEngine;

public class RoomScreenNavigator : MonoBehaviour
{
    [System.Serializable]
    public struct RoomPanelBinding
    {
        [Tooltip("room_id из JSON: 1 — Первопроходцы, 2 — Конструкторы, 3 — Программисты")]
        public int doorNumber;

        [Tooltip("Панель (GameObject) конкретной традиции")]
        public GameObject panel;

        [Tooltip("Presenter на этой панели (покажет заголовок/текст с анимацией)")]
        public RoomInfoPresenter presenter;
    }

    [Header("Panels")]
    [Tooltip("Главное меню с кнопками выбора традиции")]
    public GameObject menuPanel;

    [Tooltip("Список панелей по традициям")]
    public RoomPanelBinding[] rooms;

    // Вызывается из OnClick кнопок меню (передай 1/2/3)
    public void OpenRoomByDoor(int doorNumber)
    {
        if (!menuPanel || rooms == null || rooms.Length == 0)
        {
            Debug.LogError("[RoomScreenNavigator] Не настроены ссылки в инспекторе.");
            return;
        }

        // выключаем меню
        menuPanel.SetActive(false);

        // выключаем все панели комнат
        CloseAllRooms();

        // ищем нужную панель по номеру двери
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
                    Debug.LogWarning($"[RoomScreenNavigator] Panel {doorNumber} без RoomInfoPresenter — текст не будет показан автоматически.");
                }
                opened = true;
                break;
            }
        }

        if (!opened)
        {
            Debug.LogWarning($"[RoomScreenNavigator] Не найдена панель для doorNumber={doorNumber}. Возвращаюсь в меню.");
            BackToMenu();
        }
    }

    // Кнопка "Назад" на каждой панелі комнаты
    public void BackToMenu()
    {
        // выключаем все панели комнат
        CloseAllRooms();

        // показываем меню
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

    // опционально: открыть конструкторов/программистов/первопроходцев прямыми методами
    public void OpenPervoprohodcy() => OpenRoomByDoor(1);
    public void OpenKonstruktory() => OpenRoomByDoor(2);
    public void OpenProgrammisty() => OpenRoomByDoor(3);
}
