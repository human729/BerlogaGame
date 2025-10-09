using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> UIElements;
    [SerializeField] private GameObject EndButton;
    [SerializeField] private GameObject Story;
    [SerializeField] private TypewriterTMP typewriter;
    public static bool IsStoryEnd = false;

    private void Awake()
    {
        if (!IsStoryEnd)
        {
            Story.SetActive(true);
        }
        else if (IsStoryEnd && !typewriter.IsTyping)
        {
            foreach (GameObject obj in UIElements)
            {
                obj.SetActive(true);
            }
        }
    }

    public void TurnOnObjects()
    {
        if (EndButton.GetComponentInChildren<TextMeshProUGUI>().text == "Завершить" && !typewriter.IsTyping)
        {
            foreach (GameObject obj in UIElements)
            {
                obj.SetActive(true);
                IsStoryEnd = true;
            }
        }
    }
}
