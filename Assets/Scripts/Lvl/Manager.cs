using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GameObject Door, AndDoor, OrDoor, Lamp, BigTemp;
    public Text Temperature, DoorOpenInfo;
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

        DoorOpenInfo.text = $"<color=#93AF48>Главная:</color> T = {mainTargetTemp}°C, цвет = {mainTargetColor}\n" +
                            $"<color=#93AF48>Остальные:</color> T = {otherTargetTemp}°C, цвет = {otherTargetColor}";
    }

    void Update()
    {
        currentTemp = int.Parse(Temperature.text.Replace("°C", ""));
        Color currentColor = Lamp.GetComponent<SpriteRenderer>().color;

        ColorUtility.TryParseHtmlString(mainTargetColor, out Color mainColor);
        ColorUtility.TryParseHtmlString(otherTargetColor, out Color otherColor);

        bool mainCondition = currentColor == mainColor && currentTemp == mainTargetTemp;
        bool andCondition = currentColor == otherColor && currentTemp == otherTargetTemp;
        bool orCondition = currentColor == otherColor || currentTemp == otherTargetTemp;

        SetDoorIsOpen(Door, mainCondition);
        SetDoorIsOpen(AndDoor, andCondition);
        SetDoorIsOpen(OrDoor, orCondition);
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
                BigTemp.SetActive(true);
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
            Temperature.text = i + "°C";
            yield return new WaitForSeconds(0.25f);
        }

        isChangingTemp = false;
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("SampleScene");
    }
}
