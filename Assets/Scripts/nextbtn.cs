
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nextbtn : MonoBehaviour
{
    public GameObject panelToClose; 
    public GameObject panelToOpen;  
    public Button switchButton;     

    void Start()
    {
        if (switchButton != null)
            switchButton.onClick.AddListener(SwitchPanels);
    }

    void SwitchPanels()
    {
        if (panelToClose != null)
            panelToClose.SetActive(false);

        if (panelToOpen != null)
            panelToOpen.SetActive(true);
    }
}
