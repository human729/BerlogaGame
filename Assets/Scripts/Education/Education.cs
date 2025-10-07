using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.Security;

public class Education : MonoBehaviour
{
    public static bool isEducationDone = false;
    private bool isActiveStatic = false;
    [SerializeField] GameObject staticObjects;
    [SerializeField] GameObject intterface;
    [SerializeField] GameObject dron;
    [SerializeField] List<Button> buttons;

    private void Start()
    {

        dron.GetComponent<Comands>().enabled = false;
        foreach (var button in buttons) {
            button.enabled = false;
        }
    }
    private void Update()
    {
        if (!isEducationDone && !isActiveStatic)
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
        else if (!isActiveStatic && Input.anyKeyDown) {
            intterface.SetActive(false);
            dron.GetComponent<Comands>().enabled = true;
            foreach (var button in buttons)
            {
                button.enabled = true;
            }
        }
    }
}
