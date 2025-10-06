using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lvl3Manager : MonoBehaviour
{
    public Animator DoorAnimator;
    public Animator ElevatorAnimator;
    public Animator Elevator2Animator;
    public Animator PlatformAnimator;

    public SpriteRenderer Lamp;
    public SpriteRenderer Lamp2;
    public GameObject BigTemp;
    public Text TemperatureText, DoorOpenInfo;
    public InputField InputTemp;

    private string mainTargetColor, otherTargetColor, elevator2TargetColor, platformTargetColor;
    private int mainTargetTemp, elevatorTargetTemp;
    private int currentTemp;
    private bool isChangingTemp = false;
    private bool elevatorActivated = false;
    private bool elevator2Activated = false;
    private bool platformActivated = false;
    private bool doorActivated = false;

    void Start()
    {
        GenerateConditions();
        InputTemp.contentType = InputField.ContentType.IntegerNumber;
        if (InputTemp.placeholder != null)
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

        elevator2TargetColor = mainTargetColor;

        string[] availableColors = GetColorsExcept(mainTargetColor, colors);
        platformTargetColor = availableColors[Random.Range(0, availableColors.Length)];
        mainTargetTemp = Random.Range(0, 31);
        elevatorTargetTemp = Random.Range(0, 31);

        if (DoorOpenInfo != null)
        {
            DoorOpenInfo.text =
                $"<color=#1164B4>√Î‡‚Ì‡ˇ ‰‚Â¸:</color> T = {mainTargetTemp}∞C, À1 = {mainTargetColor}, À2 = {otherTargetColor}\n" +
                $"<color=#1164B4>ÀËÙÚ 1:</color> T = {elevatorTargetTemp}∞C " +
                $"<color=#1164B4>ÀËÙÚ 2:</color> À1 = {elevator2TargetColor}\n" +
                $"<color=#1164B4>œÎ‡ÚÙÓÏ‡:</color> À2 = {platformTargetColor}";
        }
    }
    string[] GetColorsExcept(string excludeColor, string[] allColors)
    {
        System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();
        foreach (string color in allColors)
        {
            if (color != excludeColor)
                result.Add(color);
        }
        return result.ToArray();
    }

    void Update()
    {
        if (!int.TryParse(TemperatureText.text.Replace("∞C", ""), out currentTemp)) return;

        Color currentLamp1Color = Lamp.color;
        Color currentLamp2Color = Lamp2 != null ? Lamp2.color : currentLamp1Color;

        ColorUtility.TryParseHtmlString(mainTargetColor, out Color mainColor);
        ColorUtility.TryParseHtmlString(otherTargetColor, out Color otherColor);
        ColorUtility.TryParseHtmlString(elevator2TargetColor, out Color elevator2Color);
        ColorUtility.TryParseHtmlString(platformTargetColor, out Color platformColor);

        bool doorCondition = currentLamp1Color == mainColor && currentLamp2Color == otherColor && currentTemp == mainTargetTemp;
        bool elevator1Condition = currentTemp == elevatorTargetTemp;
        bool elevator2Condition = currentLamp1Color == elevator2Color;
        bool platformCondition = currentLamp2Color == platformColor;

        if (doorCondition && !doorActivated)
        {
            if (DoorAnimator != null)
            {
                DoorAnimator.SetBool("IsOpen", true);
                doorActivated = true;
            }
        }
        else if (!doorCondition && doorActivated)
        {
            if (DoorAnimator != null)
            {
                DoorAnimator.SetBool("IsOpen", false);
                doorActivated = false;
            }
        }

        if (elevator1Condition && !elevatorActivated)
        {
            if (ElevatorAnimator != null)
            {
                ElevatorAnimator.SetBool("IsGoing", true);
                elevatorActivated = true;
            }
        }
        else if (!elevator1Condition && elevatorActivated)
        {
            if (ElevatorAnimator != null)
            {
                ElevatorAnimator.SetBool("IsGoing", false);
                elevatorActivated = false;
            }
        }

        if (elevator2Condition && !elevator2Activated)
        {
            if (Elevator2Animator != null)
            {
                Elevator2Animator.SetBool("IsGoing", true);
                elevator2Activated = true;
            }
        }
        else if (!elevator2Condition && elevator2Activated)
        {
            if (Elevator2Animator != null)
            {
                Elevator2Animator.SetBool("IsGoing", false);
                elevator2Activated = false;
            }
        }

        if (platformCondition && !platformActivated)
        {
            if (PlatformAnimator != null)
            {
                PlatformAnimator.SetBool("IsGoing", true);
                platformActivated = true;
            }
        }
        else if (!platformCondition && platformActivated)
        {
            if (PlatformAnimator != null)
            {
                PlatformAnimator.SetBool("IsGoing", false);
                platformActivated = false;
            }
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
            TemperatureText.text = i + "∞C";
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