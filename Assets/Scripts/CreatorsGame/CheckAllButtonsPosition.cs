using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class CheckAllButtonsPosition : MonoBehaviour
{
    [SerializeField] List<DragSprite> allTriiger;
    private bool allButtonsOnTheirPosition = false;
    void Update()
    {
        CheckButtons();
        if (allButtonsOnTheirPosition && Input.GetKeyDown(KeyCode.Return))
        {
            print("Buttons on right postion");
        }   
    }

    void CheckButtons()
    {
        foreach (var button in allTriiger)
        {
            if (!button.isRightPosition) {
                allButtonsOnTheirPosition = false;
                return;
            }
        }
        allButtonsOnTheirPosition = true;
    }
}
