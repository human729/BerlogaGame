using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class FlaskHandle : MonoBehaviour
{
    public FlaskScript FlaskScript;

    public void ClearLayers()
    {
        FlaskScript.LayerColors.Clear();
    }

    public void onFlaskClick()
    {
        if (FlaskScript.LayerColors.Count != FlaskScript.FlaskLayers.Count)
        {
            Color color;
            switch (gameObject.GetComponentInChildren<Text>().text)
            {
                case "Cu":
                    ColorUtility.TryParseHtmlString("#C88857", out color);
                    FlaskScript.LayerColors.Push(color);
                    FlaskScript.FlaskLayers[FlaskScript.LayerColors.Count() - 1].color = FlaskScript.LayerColors.Peek();
                    break;
                case "Fe":
                    ColorUtility.TryParseHtmlString("#D9D9D9", out color);
                    FlaskScript.LayerColors.Push(color);
                    FlaskScript.FlaskLayers[FlaskScript.LayerColors.Count() - 1].color = FlaskScript.LayerColors.Peek();
                    break;
            }
        }
    }
}
