using UnityEngine;
using UnityEngine.UI;

public class ClearButton : MonoBehaviour
{
    public void OnClearClick()
    {
        MixingController.Instance.ClearMainFlask();
    }
}