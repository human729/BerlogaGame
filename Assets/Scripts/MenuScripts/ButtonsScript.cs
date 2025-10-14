using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    [SerializeField] private GameObject SettingsMenu;
    public void StartGame()
    {
        SceneManager.LoadScene("ChoiseTradition");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
