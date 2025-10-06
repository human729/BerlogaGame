using UnityEngine;
using System.Collections;

public class DronPlace : MonoBehaviour
{
    [SerializeField]Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Dron")
        {
            animator.SetBool("DronInTrigger", true);
           
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Dron"){
            animator.SetBool("DronInTrigger", false);
        }
    }
}