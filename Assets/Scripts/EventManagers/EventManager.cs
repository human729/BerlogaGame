using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class EventManager : MonoBehaviour, ICheckConditions
{
    public GameObject Door;
    public GameObject movingFloor;
    public GameObject EndDoor;
    public Animator animator;
    public CanvasScript canvasScript;

    private int SucceedQueries = 0;
    public void CheckConditions()
    {
        Color color;
        switch (SucceedQueries)
        {
            case 0:
            StartAnimation();
            SucceedQueries++;
                break;
            case 1:
                Door.GetComponent<BoxCollider2D>().enabled = false;
                ColorUtility.TryParseHtmlString("#7B97A7", out color);
                Door.GetComponent<SpriteRenderer>().color = color;
                SucceedQueries++;
                break;
            case 2:
                EndDoor.GetComponent<BoxCollider2D>().enabled = false;
                ColorUtility.TryParseHtmlString("#7B97A7", out color);
                EndDoor.GetComponent<SpriteRenderer>().color = color;
                movingFloor.GetComponent<Animation>().Play();
                break;
            default:
                throw new Exception("No such case");
        }
    }

    public void StartAnimation()
    {
        animator.SetBool("isMoving", true);
    }
}
