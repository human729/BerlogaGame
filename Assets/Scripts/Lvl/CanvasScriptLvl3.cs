using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CanvasScriptLvl3 : MonoBehaviour
{
    [SerializeField] private GameObject Lamp1;
    [SerializeField] private GameObject Lamp2;
    [SerializeField] private GameObject TempSettings;
    [SerializeField] private GameObject ColorSettings;
    private GameObject currentLamp;

    public void ChangeLampColor(string colorString)
    {
        if (currentLamp == null) return;

        Color color;
        ColorUtility.TryParseHtmlString(colorString, out color);
        currentLamp.GetComponent<SpriteRenderer>().color = color;
    }

    public void ExitMenu()
    {
        if (TempSettings != null) TempSettings.SetActive(false);
        if (ColorSettings != null) ColorSettings.SetActive(false);
    }

    public void SetCurrentLamp(GameObject lamp)
    {
        currentLamp = lamp;
        print($"Current lamp set to: {lamp.name}");
    }
}