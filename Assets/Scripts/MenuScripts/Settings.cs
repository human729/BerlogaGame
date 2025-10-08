using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject SettingsMenu;
    public void ToggleSettingsMenu()
    {
        if (SettingsMenu.activeInHierarchy)
        {
            SettingsMenu.SetActive(false);
        }
        else
        {
            SettingsMenu.SetActive(true);
        }
    }
}
