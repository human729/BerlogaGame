using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    [SerializeField] private GameObject SettingsMenu;
    public void StartGame()
    {
        SceneManager.LoadScene("FirstLevel");
    }

    public void ToggleSettingsMenu()
    {
        if (SettingsMenu.activeInHierarchy)
        {
            SettingsMenu.SetActive(false);
        } else
        {
            SettingsMenu.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
