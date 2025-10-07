using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevell : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("DronInTrigger", false);
            SceneManager.LoadScene(sceneName);
        }
    }
}
