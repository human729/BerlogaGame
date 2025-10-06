using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Mono.Cecil;
using Mono.Data.Sqlite;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public GameObject StoryLine;
    public GameObject eventManager;
    public string answer;
    [SerializeField] private GameObject HelpMenu;

    public List<string> pastTasks = new List<string>();
    public Text TaskField;
    public Text resultField;
    [SerializeField] private DatabaseController databaseController;
    [SerializeField] private GameObject Door;
    [SerializeField] private GameObject ExitDoor;
    [SerializeField] private Queue<GameObject> TablePCs = new Queue<GameObject>();

    public Button buttonPrefab;
    public ICheckConditions checkConditions;
    public List<GameObject> PCs = new List<GameObject>();
    public GameObject parentGameObject;
    [SerializeField] private MinigameSQL SQLLogic;
    public List<Button> codeButtons = new List<Button>();
    public InputField inputField;
    public List<string> answerStrings;
    public GameObject minigameObject;

    private void Start()
    {
        checkConditions = eventManager.GetComponent<ICheckConditions>();
        foreach (GameObject PC in PCs)
        {
            TablePCs.Enqueue(PC);
            print(PC.name);
        }
    }

    public void CreateCodeButtons()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < answerStrings.Count(); i++)
        {
            Button button = Instantiate(buttonPrefab, parentGameObject.transform, false);
            button.GetComponentInChildren<Text>().text = answerStrings[i];
            button.onClick.AddListener(() =>
            {
                inputField.text += button.GetComponentInChildren<Text>().text + " ";
            });
            codeButtons.Add(button);
        };
        codeButtons = codeButtons.OrderBy(x => random.Next()).ToList();
        for (int i = 0; i < answerStrings.Count(); i++)
        {
            codeButtons[i].transform.SetSiblingIndex(i);
        }
    }

    public void ExitMenu()
    {
        inputField.text = "";
        resultField.text = "";
        for (int i = 0; i < codeButtons.Count(); i++)
        {
            Destroy(codeButtons[i].gameObject);
        }
        codeButtons = new List<Button>();
        minigameObject.SetActive(false);
    }

    public void ToggleHelp()
    {
        if (HelpMenu.activeInHierarchy)
        {
            HelpMenu.SetActive(false);
        } else
        {
            HelpMenu.SetActive(true);
        }
    }

    public void SendQuery()
    {
        if (inputField.text != "")
        {
            string query = inputField.text;
            try
            {
                DataTable table = databaseController.SelectData(query);
                DataRow[] rows = table.Select();
                resultField.text = string.Empty;
                foreach (DataRow row in rows)
                {
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        resultField.text += $"{row[i]}, ";
                    }
                    resultField.text += "\n";
                }
                if (query.ToLower().Remove(query.Length - 1) == answer.ToLower())
                {
                    StoryLine.SetActive(true);
                    pastTasks.Add(TaskField.text);
                    checkConditions.CheckConditions();
                    TablePCs.Peek().gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    TablePCs.Dequeue();
                }
            }
            catch (SqliteException exception)
            {
                print(exception.Message);
                resultField.text = "Wrong syntax or query";
            }
        }
        else
        {
            return;
        }
    }

    public void Delete()
    {
        inputField.text = "";
        resultField.text = "";
    }

    public void DeleteWord()
    {
        if (inputField.text == "")
        {
            return;
        }
        string text = inputField.text;
        List<string> words = text.Split(" ").ToList();

        inputField.text = "";
        words.Remove(words.Last());
        words.Remove(words.Last());
        
        for (int i = 0; i < words.Count; i++)
        {
            inputField.text += words[i] + " ";
        }
    }
}
