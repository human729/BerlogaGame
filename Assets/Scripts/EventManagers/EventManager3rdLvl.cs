using System;
using UnityEngine;

public class EventManager3rdLvl : MonoBehaviour, ICheckConditions
{
    [SerializeField] private Animator ElevatorAnimator;
    [SerializeField] private Animator GatesAnimator;
    [SerializeField] private GameObject FirstDoor;
    [SerializeField] private GameObject SecondDoor;
    [SerializeField] private int SucceedQueries = 0;
    public void CheckConditions()
    {
        Color color;
        switch (SucceedQueries)
        {
            case 0:
                FirstDoor.GetComponent<BoxCollider2D>().enabled = false;
                ColorUtility.TryParseHtmlString("#7B97A7", out color);
                FirstDoor.GetComponent<SpriteRenderer>().color = color;
                SucceedQueries++;
                break;
            case 1:
                SucceedQueries++;
                break;
            case 2:
                ElevatorAnimator.SetBool("isMoving", true);
                SucceedQueries++;
                break;
            case 3:
                SecondDoor.GetComponent<BoxCollider2D>().enabled = false;
                ColorUtility.TryParseHtmlString("#7B97A7", out color);
                SecondDoor.GetComponent<SpriteRenderer>().color = color;
                SucceedQueries++;
                break;
            case 4:
                GatesAnimator.SetBool("isOpeningGates", true);
                break;
            default:
                throw new Exception("No such action");
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
