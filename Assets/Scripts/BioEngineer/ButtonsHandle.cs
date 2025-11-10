using UnityEngine;

public class ButtonsHandle : MonoBehaviour
{
    public GameObject MinigameTab;
    public void CloseTab()
    {
        MinigameTab.SetActive(false);
    }
}
