using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Education4Programmers  : MonoBehaviour
{
    [SerializeField] GameObject InterfaceUI;
    [SerializeField] GameObject Story;
    void Awake()
    {
        if (!Story.activeInHierarchy)
        {
            InterfaceUI.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown) 
        { 
            InterfaceUI.SetActive(false);
          
        }
    }
}
