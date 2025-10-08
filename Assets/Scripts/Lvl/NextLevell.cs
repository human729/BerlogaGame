using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NextLevell : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] List<Animator> animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var animtor in animator) {
                animtor.SetBool("DronInTrigger", false);
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
