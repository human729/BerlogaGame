using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.Security;

public class Education : MonoBehaviour
{
    public static bool isEducationDone = false;
    private bool isActiveStatic = false;
    private bool isActiveStartHint = false;
    private bool isInterfaceActive = false;
    [SerializeField] GameObject staticObjects;
    [SerializeField] GameObject intterface;
    [SerializeField] GameObject dron;
    [SerializeField] List<Button> buttons;
    [SerializeField] GameObject StartHint;

    private void Start()
    {
        dron.GetComponent<Comands>().enabled = false;
        foreach (var button in buttons) {
            button.enabled = false;
        }
    }
    private void Update()
    {
        if (!isEducationDone && !isActiveStatic && !isActiveStartHint && !isInterfaceActive)
        {
            staticObjects.SetActive(true);
            isActiveStatic = true;
        }
        else if (isActiveStatic && Input.anyKeyDown)
        {
            staticObjects.SetActive(false);
            isActiveStatic = false;
            isEducationDone = true;
            intterface.SetActive(true);
        }
        else if (isEducationDone && !isActiveStartHint && Input.anyKeyDown)
        {
            isActiveStartHint = true;
            isInterfaceActive = false;
            intterface.SetActive(false);
            StartHint.SetActive(true);
        }
        else if (isActiveStartHint && Input.anyKeyDown) {
            StartHint.SetActive(false);
            dron.GetComponent<Comands>().enabled = true;
            foreach (var button in buttons)
            {
                button.enabled = true;
            }
        }
    }
}
