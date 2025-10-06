using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CanvasScriptTemp : MonoBehaviour
{
    [SerializeField] private GameObject Lamp;
    [SerializeField] private GameObject TempSettings;
    [SerializeField] private GameObject ColorSettings;
    public void ChangeLampColor(string colorString)
    {
        Color color;
        ColorUtility.TryParseHtmlString(colorString, out color);
        Lamp.GetComponent<SpriteRenderer>().color = color;  
    }

    public void ExitMenu()
    {
        if (TempSettings != null) TempSettings.SetActive(false);
        if (ColorSettings != null) ColorSettings.SetActive(false);
    }
}
