using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NextLv : MonoBehaviour
{
    [SerializeField] string sceneName;
    
    public void NextLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
