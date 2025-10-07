using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MinigameSQL : MonoBehaviour
{
    public int SceneNumber;
    [SerializeField] private CanvasScript canvasScript;
    public InputField queryField;
    public Text TaskField;
    public Text resultField;
    [SerializeField] private DatabaseController databaseController;
    private DataRow taskAndAnswer;
    public string answer;

    public GameObject Player;

    public void CreateTask()
    {
        int triesCount = 0;
        do
        {
            triesCount++;
            CreateTaskAndAnswer();
            if (triesCount >= 50)
            {
                throw new Exception("Too much tries");
            }
        } while (canvasScript.pastTasks.Contains(taskAndAnswer["task"]));
        List<string> answerString = answer.Split(" ").ToList();
        canvasScript.answer = answer;
        canvasScript.answerStrings = answerString;
        if (!canvasScript.codeButtons.Any())
        {
            canvasScript.CreateCodeButtons();
        }
        TaskField.text = $"{taskAndAnswer["task"]}";
    }

    private void CreateTaskAndAnswer()
    {
        DataTable taskTable;
        switch (SceneNumber)
        {
            case 1:
                taskTable = databaseController.SelectData("SELECT * FROM Tasks_and_answers WHERE SceneNumber = 1 ORDER BY RANDOM() LIMIT 1");
                taskAndAnswer = taskTable.Rows[0];
                break;
            case 2:
                taskTable = databaseController.SelectData("SELECT * FROM Tasks_and_answers WHERE SceneNumber = 2 ORDER BY RANDOM() LIMIT 1");
                taskAndAnswer = taskTable.Rows[0];
                break;
            case 3:
                taskTable = databaseController.SelectData("SELECT * FROM Tasks_and_answers WHERE SceneNumber = 3 ORDER BY RANDOM() LIMIT 1");
                taskAndAnswer = taskTable.Rows[0];
                break;
            default:
                throw new Exception("No such scene");
        }
        answer = taskAndAnswer["answer"].ToString();
    }
}
