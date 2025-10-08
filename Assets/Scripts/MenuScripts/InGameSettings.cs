using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSettings : MonoBehaviour
{
    public GameObject EscapePanel;
    public CharacterController controller;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapePanel.SetActive(true);
            controller.enabled = false;
        }
    }

    public void ContinueGame()
    {
        EscapePanel.SetActive(false);
        controller.enabled = true;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
