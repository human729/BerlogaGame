using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Education4Programmers  : MonoBehaviour
{
    [SerializeField] GameObject InterfaceUI;
    void Start()
    {
        InterfaceUI.SetActive(true);
    }

    void Update()
    {
        if (Input.anyKeyDown) 
        { 
            InterfaceUI.SetActive(false);
            enabled = false;
        }
    }
}
