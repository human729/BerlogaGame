using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �������� ������������ ����: �� �������/�������
/// �������� ����� ��������� ����� � RoomTextController.
/// </summary>
public class RoomTaskTriggers : MonoBehaviour
{
    [Serializable]
    public class TaskFlag
    {
        public int taskNumber;
        public bool showMessage;     // �������� message
        public bool showAfterPassing; // �������� after_passing (����� ������)
        [NonSerialized] public bool _usedMsg;
        [NonSerialized] public bool _usedAfter;
    }

    [Header("������ �� ���������� �������")]
    public RoomTextController controller;

    [Header("����� ������ ���� ������� �������� �����")]
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

        // ������� ������: ���� � ���������� �������� ������� � ���� ��� ������� �����
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

        // ������: ������� ������� ��� ����� (����� ������)
        if (Input.GetKeyDown(KeyCode.Alpha1))
            controller.ShowTaskMessage(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            controller.ShowTaskMessage(2);
        if (Input.GetKeyDown(KeyCode.F1))
            controller.ShowTaskAfterPassing(1);
        if (Input.GetKeyDown(KeyCode.F2))
            controller.ShowTaskAfterPassing(2);
    }

    // ��������� ������ � ������ ������ �� UI-������
    public void TriggerTaskMessage(int taskNumber) => controller?.ShowTaskMessage(taskNumber);
    public void TriggerTaskAfter(int taskNumber) => controller?.ShowTaskAfterPassing(taskNumber);
}
