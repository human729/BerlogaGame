using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lvl1Manager : MonoBehaviour
{
    public OpenDoor Door;
    public Animator ElevatorAnimator;

    public SpriteRenderer Lamp;
    public GameObject BigTemp;
    public Text TemperatureText, DoorOpenInfo;
    public InputField InputTemp;

    private string mainTargetColor, otherTargetColor, elevatorTargetColor;
    private int mainTargetTemp, otherTargetTemp;
    private int currentTemp;
    private bool isChangingTemp = false;
    private bool elevatorActivated = false;

    void Start()
    {
        GenerateConditions();
        InputTemp.contentType = InputField.ContentType.IntegerNumber;
        InputTemp.placeholder.GetComponent<Text>().text = "";
    }

    void GenerateConditions()
    {
        string[] colors = { "#FF0000", "#0000FF", "#00FF00" };

        mainTargetColor = colors[Random.Range(0, colors.Length)];
        do
        {
            otherTargetColor = colors[Random.Range(0, colors.Length)];
        } while (otherTargetColor == mainTargetColor);

        do
        {
            elevatorTargetColor = colors[Random.Range(0, colors.Length)];
        } while (elevatorTargetColor == mainTargetColor || elevatorTargetColor == otherTargetColor);

        mainTargetTemp = Random.Range(0, 31);
        otherTargetTemp = Random.Range(0, 31);

        if (DoorOpenInfo != null)
        {
            DoorOpenInfo.text =
                $"<color=#1164B4>Главная:</color> T = {mainTargetTemp}°C, цвет = {mainTargetColor}\n" +
                $"<color=#1164B4>Лифт:</color> цвет = {elevatorTargetColor}";
        }
    }

    void Update()
    {
        if (!int.TryParse(TemperatureText.text.Replace("°C", ""), out currentTemp)) return;
        Color currentColor = Lamp.color;

        ColorUtility.TryParseHtmlString(mainTargetColor, out Color mainColor);
        ColorUtility.TryParseHtmlString(elevatorTargetColor, out Color elevatorColor);

        bool mainCondition = currentColor == mainColor && currentTemp == mainTargetTemp;
        bool elevatorCondition = currentColor == elevatorColor;

        if (Door != null) Door.SetOpen(mainCondition);

        if (elevatorCondition && !elevatorActivated)
        {
            ActivateElevator();
        }
        else if (!elevatorCondition && elevatorActivated)
        {
            DeactivateElevator();
        }
    }

    void ActivateElevator()
    {
        if (ElevatorAnimator != null)
        {
            ElevatorAnimator.SetBool("IsGoing", true);
            elevatorActivated = true;
            Debug.Log("Лифт активирован!");
        }
    }

    void DeactivateElevator()
    {
        if (ElevatorAnimator != null)
        {
            ElevatorAnimator.SetBool("IsGoing", false);
            elevatorActivated = false;
            Debug.Log("Лифт деактивирован!");
        }
    }

    public void ChangeTemp()
    {
        if (int.TryParse(InputTemp.text, out int newTemp))
        {
            if (newTemp >= 35)
            {
                BigTemp?.SetActive(true);
                StartCoroutine(RestartScene());
            }

            if (!isChangingTemp)
                StartCoroutine(SmoothTempChange(currentTemp, newTemp));
        }
    }

    IEnumerator SmoothTempChange(int from, int to)
    {
        isChangingTemp = true;
        int step = from < to ? 1 : -1;

        for (int i = from; i != to + step; i += step)
        {
            TemperatureText.text = i + "°C";
            yield return new WaitForSeconds(0.25f);
        }

        isChangingTemp = false;
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}