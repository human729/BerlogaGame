using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChange : MonoBehaviour
{
    public TextMeshProChanger textChanger;

    public void OnButtonClick()
    {
        textChanger.ChangeText("Текст после нажатия кнопки");
    }
}