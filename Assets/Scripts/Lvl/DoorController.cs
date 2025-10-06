using UnityEngine;
using System.Collections;

public class DronPlace : MonoBehaviour
{
    [SerializeField]Animator animator;
    [SerializeField] AudioSource source;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Dron")
        {
            animator.SetBool("DronInTrigger", true);
            source.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Dron"){
            animator.SetBool("DronInTrigger", false);
        }
    }
}