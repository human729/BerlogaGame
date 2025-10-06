using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum LevelType
{
    OneDoor_OneCondition,
    TwoDoors,
    OneDoor_ThreeConditions
}

public class ConditionsGen : MonoBehaviour
{
    public LevelType levelType;

    public GameObject Door;
    public GameObject AndDoor;
    public GameObject OrDoor;
    public GameObject Lamp;
    public GameObject BigTemp;

    public Text Temperature;
    public Text DoorOpenInfo;
    public InputField InputTemp;

    private string mainTargetColor, otherTargetColor;
    private int mainTargetTemp, otherTargetTemp;
    private int currentTemp;
    private bool isChangingTemp = false;

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
        } while (mainTargetColor == otherTargetColor);

        mainTargetTemp = Random.Range(0, 31);
        otherTargetTemp = Random.Range(0, 31);

        if (DoorOpenInfo != null)
        {
            DoorOpenInfo.text = $"<color=#1164B4>√Î‡‚Ì‡ˇ:</color> T = {mainTargetTemp}∞C, ˆ‚ÂÚ = {mainTargetColor}\n" +
                                $"<color=#1164B4>ŒÒÚ‡Î¸Ì˚Â:</color> T = {otherTargetTemp}∞C, ˆ‚ÂÚ = {otherTargetColor}";
        }
    }

    void Update()
    {
        if (!int.TryParse(Temperature.text.Replace("∞C", ""), out currentTemp)) return;

        Color currentColor = Lamp.GetComponent<SpriteRenderer>().color;

        ColorUtility.TryParseHtmlString(mainTargetColor, out Color mainColor);
        ColorUtility.TryParseHtmlString(otherTargetColor, out Color otherColor);

        bool condition1 = currentColor == mainColor;
        bool condition2 = currentTemp == mainTargetTemp;
        bool condition3 = currentColor == otherColor || currentTemp == otherTargetTemp;

        switch (levelType)
        {
            case LevelType.OneDoor_OneCondition:
                if (Door != null)
                    SetDoorIsOpen(Door, condition1 && condition2);
                break;

            case LevelType.TwoDoors:
                if (Door != null)
                    SetDoorIsOpen(Door, condition1 && condition2);
                if (AndDoor != null)
                    SetDoorIsOpen(AndDoor, currentColor == otherColor && currentTemp == otherTargetTemp);
                if (OrDoor != null)
                    SetDoorIsOpen(OrDoor, currentColor == otherColor || currentTemp == otherTargetTemp);
                break;

            case LevelType.OneDoor_ThreeConditions:
                if (Door != null)
                    SetDoorIsOpen(Door, condition1 && condition2 && condition3);
                break;
        }
    }

    void SetDoorIsOpen(GameObject door, bool isOpen)
    {
        Animator animator = door.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsOpen", isOpen);
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
            Temperature.text = i + "∞C";
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
