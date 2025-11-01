using UnityEngine;
using System.Collections.Generic;

public class MixingController : MonoBehaviour
{
    public static MixingController Instance;

    [Header("References")]
    public MainFlask mainFlask;
    public List<ReagentFlask> reagentFlasks;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mainFlask.TryMixAllElements();
        }
    }

    public void AddReagentToMainFlask(string elementName, Color elementColor)
    {
        mainFlask.AddElement(elementName, elementColor);
    }

    public void ClearMainFlask()
    {
        mainFlask.ClearFlask();
    }
}