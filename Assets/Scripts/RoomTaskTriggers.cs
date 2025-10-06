using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пример внешнего управляющего кода: по флажкам/методам
/// вызывает показ сообщений задач в RoomTextController.
/// </summary>
public class RoomTaskTriggers : MonoBehaviour
{
    [Serializable]
    public class TaskFlag
    {
        public int taskNumber;
        public bool showMessage;     // показать message
        public bool showAfterPassing; // показать after_passing (когда готово)
        [NonSerialized] public bool _usedMsg;
        [NonSerialized] public bool _usedAfter;
    }

    [Header("Ссылка на контроллер комнаты")]
    public RoomTextController controller;

    [Header("Какие задачи этой комнаты включать извне")]
    public List<TaskFlag> flags = new()
    {
        new TaskFlag{ taskNumber = 1, showMessage = false, showAfterPassing = false },
        new TaskFlag{ taskNumber = 2, showMessage = false, showAfterPassing = false },
    };

    private void Reset()
    {
        controller = FindObjectOfType<RoomTextController>();
    }

    private void Update()
    {
        if (controller == null) return;

        // Простая модель: если в инспекторе включили галочку — один раз вызовем показ
        foreach (var f in flags)
        {
            if (f.showMessage && !f._usedMsg)
            {
                controller.ShowTaskMessage(f.taskNumber);
                f._usedMsg = true;
            }
            if (f.showAfterPassing && !f._usedAfter)
            {
                controller.ShowTaskAfterPassing(f.taskNumber);
                f._usedAfter = true;
            }
        }

        // Пример: горячие клавиши для теста (можно убрать)
        if (Input.GetKeyDown(KeyCode.Alpha1))
            controller.ShowTaskMessage(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            controller.ShowTaskMessage(2);
        if (Input.GetKeyDown(KeyCode.F1))
            controller.ShowTaskAfterPassing(1);
        if (Input.GetKeyDown(KeyCode.F2))
            controller.ShowTaskAfterPassing(2);
    }

    // Публичные методы — удобно вешать на UI-кнопки
    public void TriggerTaskMessage(int taskNumber) => controller?.ShowTaskMessage(taskNumber);
    public void TriggerTaskAfter(int taskNumber) => controller?.ShowTaskAfterPassing(taskNumber);
}
