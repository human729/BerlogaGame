using System;
using System.Collections;
using UnityEngine;

public class LeverElevator : MonoBehaviour
{
    [SerializeField] Animator ElevatorAnimator;
    bool elevatorWork = false;
    bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!elevatorWork)
            {
                ElevatorAnimator.SetBool("isButtonOn", true);
                elevatorWork = true;
                transform.rotation = new Quaternion(0,180,0, 1);
            }
            else
            {
                ElevatorAnimator.SetBool("isButtonOn", false);
                elevatorWork = false;
                transform.rotation = new Quaternion(0, 0, 0, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerInRange = false;
        }
    }



}
