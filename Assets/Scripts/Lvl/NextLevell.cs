using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NextLevell : MonoBehaviour
{
    [SerializeField] List<Animator> animator;
    [SerializeField] SceneLoader sceneLoader;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var animtor in animator) {
                animtor.SetBool("DronInTrigger", false);
            }

            sceneLoader.LoadNextScene();
            CanvasManager.IsStoryEnd = false;
        }
    }
}
