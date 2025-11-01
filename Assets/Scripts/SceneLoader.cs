using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private List<int[]> SceneNumbers = new List<int[]>();
    public static Queue<int[]> Scenes = new Queue<int[]>();
    private int[] ResearcherScenes = { 2, 3, 4 };
    private int[] ConstructorScenes = { 5, 6, 7 };
    private int[] ProgrammerScenes = { 8, 9, 10 };
    private int[] BioengineerScenes = { 14 };
    private int[] BeekeeperScenes = { 13 };
    private int[] CreatorScenes = { 15 };

    public void AddScenesToList()
    {
        SceneNumbers.Add(ResearcherScenes);
        SceneNumbers.Add(ConstructorScenes);
        SceneNumbers.Add(ProgrammerScenes);
        SceneNumbers.Add(BioengineerScenes);
        SceneNumbers.Add(BeekeeperScenes);
        SceneNumbers.Add(CreatorScenes);
        string SceneName = gameObject.GetComponentInChildren<Text>().text;

        switch (SceneName)
        {
            case "Конструкторы":
                (SceneNumbers[0], SceneNumbers[1]) = (SceneNumbers[1], SceneNumbers[0]);
                break;

            case "Программисты":
                (SceneNumbers[0], SceneNumbers[2]) = (SceneNumbers[2], SceneNumbers[0]);
                break;

            case "Биоинженеры":
                (SceneNumbers[0], SceneNumbers[3]) = (SceneNumbers[3], SceneNumbers[0]);
                break;

            case "Пчеловоды":
                (SceneNumbers[0], SceneNumbers[4]) = (SceneNumbers[4], SceneNumbers[0]);
                break;

            case "Творцы":
                (SceneNumbers[0], SceneNumbers[5]) = (SceneNumbers[5], SceneNumbers[0]);
                break;

            case "Первопроходцы":
                break;

            default:
                print("No such scene");
                break;
        }

        foreach (int[] sceneNumber in SceneNumbers)
        {
            Scenes.Enqueue(sceneNumber);
        }
    }

    public void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int[] LevelScenes = Scenes.Peek();
        if (currentScene == 11)
        {
            int firstScene = LevelScenes[0];
            TransitionLvl.Instance.LoadSceneWith(firstScene);
        }
        if (currentScene == LevelScenes.Last())
        {
            Scenes.Dequeue();
            if (!Scenes.Any())
            {
                TransitionLvl.Instance.LoadSceneWith("EndScene");
                return;
            }
            LevelScenes = Scenes.Peek();
            TransitionLvl.Instance.LoadSceneWith(LevelScenes[0]);
        }
        else
        {
            TransitionLvl.Instance.LoadSceneWith(++currentScene);
        }
    }
}
